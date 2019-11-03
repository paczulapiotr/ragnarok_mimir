using Microsoft.EntityFrameworkCore;
using Mimir.Database;
using System;
using System.Linq;

namespace Mimir.Kanban
{
    public class KanbanAccessService : IKanbanAccessService
    {
        private readonly MimirDbContext _dbContext;

        public KanbanAccessService(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool HasAccess(int userId, int boardId)
        {
            var userBoards = _dbContext.AppUsers.AsNoTracking()
                .Include(x => x.BoardsWithAccess)
                .Include(x => x.OwnedBoards)
                .Where(x => x.ID == userId)
                .SelectMany(x =>
                    x.BoardsWithAccess.Select(x => x.BoardID)
                    .Union(x.OwnedBoards.Select(x => x.ID)));

            return userBoards.Contains(boardId);
        }

        public bool IsOwner(int userId, int boardId)
        {
            return _dbContext.KanbanBoards
                .AsNoTracking()
                .Any(x => x.OwnerID == userId);
        }
    }
}
