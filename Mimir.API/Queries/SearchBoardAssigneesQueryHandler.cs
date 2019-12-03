using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.API.DTO;
using Mimir.CQRS.Queries;
using Mimir.Database;
using Mimir.Core.Extensions;

namespace Mimir.API.Queries
{
    public class SearchBoardAssigneesQueryHandler : IQueryHandler<PaginableList<AppUserBasicResultDTO>, SearchBoardAssigneesQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;

        public SearchBoardAssigneesQueryHandler(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginableList<AppUserBasicResultDTO>> HandleAsync(Query query)
        {
            query.Validate();
            var board = _dbContext.KanbanBoards
                .Include(x => x.Owner)
                .Include(x => x.UsersWithAccess)
                .ThenInclude(x => x.UserWithAccess)
                .FirstOrDefault(x => x.ID == query.BoardId);

            var users = board.UsersWithAccess.Select(x => x.UserWithAccess).ToHashSet();
            users.Add(board.Owner);
            var totalCount = users.Count();
            var pagesCount = query.GetPageCount(totalCount);

            return await users.OrderBy(x => x.Name)
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
            public int BoardId { get; set; }
        }
    }
}
