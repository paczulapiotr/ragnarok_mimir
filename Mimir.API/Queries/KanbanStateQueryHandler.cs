using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mimir.API.DTO;
using Mimir.CQRS.Queries;
using Mimir.Database;
using System.Threading.Tasks;

namespace Mimir.API.Queries
{
    public class KanbanStateQueryHandler : IQueryHandler<KanbanBoardResultDTO, KanbanStateQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;
        private readonly IMapper _mapper;

        public KanbanStateQueryHandler(MimirDbContext dbContext, MapperConfiguration mapperConfiguration)
        {
            _dbContext = dbContext;
            _mapper = mapperConfiguration.CreateMapper();
        }

        public async Task<KanbanBoardResultDTO> HandleAsync(Query query)
        {
            var board = await _dbContext.KanbanBoards
                .AsNoTracking()
                .Include(x => x.Columns)
                .ThenInclude(x => x.Items)
                .ThenInclude(x => x.Assignee)
                .FirstOrDefaultAsync(x => x.ID == query.BoardId);

            var result = _mapper.Map<KanbanBoardResultDTO>(board);

            return result;
        }

        public class Query : IQuery<KanbanBoardResultDTO>
        {
            public Query(int boardId)
            {
                BoardId = boardId;
            }

            public int BoardId { get; set; }
        }
    }
}
