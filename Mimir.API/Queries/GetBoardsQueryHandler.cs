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
    public class GetBoardsQueryHandler : IQueryHandler<IEnumerable<GetBoardResultDTO>, GetBoardsQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;

        public GetBoardsQueryHandler(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetBoardResultDTO>> HandleAsync(Query query)
        {
            var qry = _dbContext.KanbanBoards.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
                qry = qry.Where(x => x.Name.ToLower().Contains(query.Search));

            qry = query.Owned
                ? qry.Where(x => x.OwnerID == query.UserId)
                : qry.Include(x => x.UsersWithAccess)
                .Where(x => x.UsersWithAccess.Any(x => x.UserWithAccessID == query.UserId));

            return await qry.OrderBy(x => x.Name)
                .Paginate(query.Page, query.PageSize)
                .Select(x => new GetBoardResultDTO { Id = x.ID, Name = x.Name })
                .ToListAsync();
        }

        public class Query : IQuery<IEnumerable<GetBoardResultDTO>>
        {
            public bool Owned { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int UserId { get; set; }
            public string Search { get; set; }
        }
    }
}
