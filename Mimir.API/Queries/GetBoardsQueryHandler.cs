using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mimir.API.DTO.Board;
using Mimir.CQRS.Queries;
using Mimir.Database;
using Mimir.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Mimir.Core.Models;

namespace Mimir.API.Queries
{
    public class GetBoardsQueryHandler : IQueryHandler<PaginableList<GetBoardResultDTO>, GetBoardsQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;

        public GetBoardsQueryHandler(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginableList<GetBoardResultDTO>> HandleAsync(Query query)
        {
            query.Validate();
            var qry = _dbContext.KanbanBoards.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
                qry = qry.Where(x => x.Name.ToLower().Contains(query.Search));

            qry = query.Owned
                ? qry.Where(x => x.OwnerID == query.UserId)
                : qry.Include(x => x.UsersWithAccess)
                .Where(x => x.UsersWithAccess.Any(x => x.UserWithAccessID == query.UserId));

            var totalCount = qry.Count();
            var pageCount = query.GetPageCount(totalCount);

            return (await qry.OrderBy(x => x.Name)
                .Paginate(query.Page, query.PageSize)
                .Select(x => new GetBoardResultDTO { Id = x.ID, Name = x.Name })
                .ToListAsync())
                .ToPaginableList(query.Page, pageCount, totalCount);
        }

        public class Query : PaginationQuery<GetBoardResultDTO>
        {
            public bool Owned { get; set; }
            public int UserId { get; set; }
            public string Search { get; set; }
        }

        
    }
}
