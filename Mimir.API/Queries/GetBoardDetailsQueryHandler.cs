using System.Linq;
using System.Threading.Tasks;
using Mimir.API.DTO.Board;
using Mimir.Core.CommonExceptions;
using Mimir.CQRS.Queries;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Queries
{
    public class GetBoardDetailsQueryHandler : IQueryHandler<BoardDetailsResultDTO, GetBoardDetailsQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IKanbanAccessService _accessService;

        public GetBoardDetailsQueryHandler(MimirDbContext dbContext, 
            IQueryDispatcher queryDispatcher, 
            IKanbanAccessService accessService)
        {
            _dbContext = dbContext;
            _queryDispatcher = queryDispatcher;
            _accessService = accessService;
        }

        public async Task<BoardDetailsResultDTO> HandleAsync(Query query)
        {
            if (!_accessService.HasAccess(query.UserId, query.BoardId))
            {
                throw new ForbiddenException();
            }

            var participants = await _queryDispatcher.DispatchAsync(
                new GetBoardParticipantsQueryHandler.Query { BoardId = query.BoardId, UserId = query.UserId });
            
            var details = _dbContext.KanbanBoards
                .Select(x => new { x.ID, x.Name, x.Description })
                .FirstOrDefault(x => x.ID == query.BoardId);

            return new BoardDetailsResultDTO
            {
                Id = details.ID,
                Name = details.Name,
                Description = details.Description,
                Participants = participants
            };
        }

        public class Query : IQuery<BoardDetailsResultDTO>
        {
            public int BoardId { get; set; }
            public int UserId { get; set; }

        }
    }
}
