using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mimir.API.DTO;
using Mimir.Core.CommonExceptions;
using Mimir.CQRS.Queries;
using Mimir.Database;
using Mimir.Kanban;
using System.Threading.Tasks;

namespace Mimir.API.Queries
{
    public class KanbanStateQueryHandler : IQueryHandler<KanbanBoardResultDTO, KanbanStateQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;
        private readonly IKanbanAccessService _accessService;
        private readonly IMapper _mapper;

        public KanbanStateQueryHandler(MimirDbContext dbContext, 
            MapperConfiguration mapperConfiguration, 
            IKanbanAccessService accessService)
        {
            _dbContext = dbContext;
            _accessService = accessService;
            _mapper = mapperConfiguration.CreateMapper();
        }

        public async Task<KanbanBoardResultDTO> HandleAsync(Query query)
        {
            if (!_accessService.HasAccess(query.UserId, query.BoardId))
                throw new ForbiddenException();

            var board = await _dbContext.KanbanBoards
                .AsNoTracking()
                .Include(x => x.Columns)
                .ThenInclude(x => x.Items)
                .ThenInclude(x => x.Assignee)
                .FirstOrDefaultAsync(x => x.ID == query.BoardId);

            if (board == null)
                throw new NotFoundException("Given kanban board was not found");
            
            var result = _mapper.Map<KanbanBoardResultDTO>(board);
            result.IsOwner = board.OwnerID == query.UserId;

            return result;
        }

        public class Query : IQuery<KanbanBoardResultDTO>
        {
            public Query(int userId, int boardId)
            {
                UserId = userId;
                BoardId = boardId;
            }

            public int BoardId { get; set; }
            public int UserId { get; set; }
        }
    }
}
