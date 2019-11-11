using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.API.DTO;
using Mimir.CQRS.Queries;
using Mimir.Database;
using Mimir.Core.Extensions;

namespace Mimir.API.Queries
{
    public class SearchBoardParticipantsQueryHandler : IQueryHandler<IEnumerable<AppUserBasicDTO>, SearchBoardParticipantsQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;

        public SearchBoardParticipantsQueryHandler(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AppUserBasicDTO>> HandleAsync(Query query)
        {
            var usersAlreadyWithAccess = query.BoardId.HasValue
                ? _dbContext.KanbanBoards
                .Where(x => x.ID == query.BoardId)
                .Include(x => x.UsersWithAccess)
                .SelectMany(x=>x.UsersWithAccess.Select(x=> x.UserWithAccessID))
                .ToHashSet()
                : new HashSet<int>();
            usersAlreadyWithAccess.Add(query.UserId);

            if (query.IgnoreUserIds != null)
                usersAlreadyWithAccess.UnionWith(query.IgnoreUserIds);

            return await _dbContext.AppUsers
                .Where(x => !usersAlreadyWithAccess.Any(y => y == x.ID))
                .Where(x => x.Name.ToLower().Contains(query.Name.ToLower()))
                .OrderBy(x => x.Name)
                .Paginate(query.Page, query.PageSize)
                .Select(x =>
                new AppUserBasicDTO
                {
                    Id = x.ID,
                    Name = x.Name
                }).ToListAsync();
        }

        public class Query : IQuery<IEnumerable<AppUserBasicDTO>>
        {
            public int UserId { get; set; }
            public string Name { get; set; }
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 5;
            public int? BoardId { get; set; }
            public IEnumerable<int> IgnoreUserIds { get; set; } = new List<int>();
        }
    }
}
