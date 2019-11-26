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
    public class SearchBoardParticipantsQueryHandler : IQueryHandler<PaginableList<AppUserBasicResultDTO>, SearchBoardParticipantsQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;

        public SearchBoardParticipantsQueryHandler(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginableList<AppUserBasicResultDTO>> HandleAsync(Query query)
        {
            query.Validate();
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

            var qry = _dbContext.AppUsers
                .Where(x => !usersAlreadyWithAccess.Any(y => y == x.ID))
                .Where(x => x.Name.ToLower().Contains(query.Name.ToLower()));

            var totalCount = qry.Count();
            var pagesCount = query.GetPageCount(totalCount);

            return await qry.OrderBy(x => x.Name)
                .Paginate(query.Page, query.PageSize)
                .Select(x =>
                new AppUserBasicResultDTO
                {
                    Id = x.ID,
                    Name = x.Name
                }).ToPaginableListAsync(query.Page, pagesCount, totalCount);
        }

        public class Query : PaginationQuery<AppUserBasicResultDTO>
        {
            public int UserId { get; set; }
            public string Name { get; set; }
            public int? BoardId { get; set; }
            public IEnumerable<int> IgnoreUserIds { get; set; } = new List<int>();
        }
    }
}
