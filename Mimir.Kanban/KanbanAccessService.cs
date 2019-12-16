using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mimir.Core.Models;
using Mimir.Database;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Mimir.Kanban
{
    public class KanbanAccessService : IKanbanAccessService
    {
        private readonly MimirDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserResolver _userResolver;

        public KanbanAccessService(MimirDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IUserResolver userResolver)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userResolver = userResolver;
        }

        private AppUser GetUser()
        {
            return _userResolver.GetUser(_httpContextAccessor.HttpContext.User);
        }

        public bool HasAccess(int userId, int boardId)
            => HassAccessBase(user => user.ID == userId)(boardId);

        public bool HasAccess(string authId, int boardId) 
            => HassAccessBase(user => user.AuthID == authId)(boardId);

        public bool HasAccess(int boardId)
        {
            var user = GetUser();
            return HasAccess(user.ID, boardId);
        }


        public bool IsOwner(int userId, int boardId)
        {
            return _dbContext.KanbanBoards
                .AsNoTracking()
                .Any(x => x.OwnerID == userId);
        }

        public bool IsOwner(int boardId)
        {
            var user = GetUser();
            return IsOwner(user.ID, boardId);
        }

        private Func<int, bool> HassAccessBase(Expression<Func<AppUser, bool>> userSelectorExpression)
        {
            return (int boardId) =>
            {

                var userBoards = _dbContext.AppUsers.AsNoTracking()
                    .Include(x => x.BoardsWithAccess)
                    .Include(x => x.OwnedBoards)
                    .Where(userSelectorExpression)
                    .SelectMany(x =>
                        x.BoardsWithAccess.Select(x => x.BoardID)
                        .Union(x.OwnedBoards.Select(x => x.ID)));

                return userBoards.Contains(boardId);
            };
        }
    }
}