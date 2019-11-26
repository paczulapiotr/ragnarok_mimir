using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.API.DTO.Board;
using Mimir.Core.CommonExceptions;
using Mimir.CQRS.Queries;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Queries
{
    public class GetBoardBaseQueryHandler : IQueryHandler<GetBoardResultDTO, GetBoardBaseQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;
        private readonly IKanbanAccessService _accessService;

        public GetBoardBaseQueryHandler(MimirDbContext dbContext, IKanbanAccessService accessService)
        {
            _dbContext = dbContext;
            _accessService = accessService;
        }

        public async Task<GetBoardResultDTO> HandleAsync(Query query)
        {
            if (!_accessService.HasAccess(query.UserId, query.BoardId))
                throw new ForbiddenException();

            var boardBase = await _dbContext.KanbanBoards
                .Where(x => x.ID == query.BoardId)
                .Select(x => new { x.ID, x.Name })
                .FirstOrDefaultAsync();

            return new GetBoardResultDTO
            {
                Id = boardBase.ID,
                Name = boardBase.Name
            };
        }

        public class Query : IQuery<GetBoardResultDTO>
        {
            public int BoardId { get; set; }
            public int UserId { get; set; }
        }
    }
}
