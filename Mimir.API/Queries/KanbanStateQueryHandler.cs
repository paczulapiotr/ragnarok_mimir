using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mimir.API.DTO;
using Mimir.CQRS.Queries;
using Mimir.Database;
using System.Threading.Tasks;

namespace Mimir.API.Queries
{
    public class KanbanStateQueryHandler : IQueryHandler<KanbanBoardDTO, KanbanStateQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;
        private readonly IMapper _mapper;

        public KanbanStateQueryHandler(MimirDbContext dbContext, MapperConfiguration mapperConfiguration)
        {
            _dbContext = dbContext;
            _mapper = mapperConfiguration.CreateMapper();
        }

        public async Task<KanbanBoardDTO> HandleAsync(Query query)
        {
            var board = await _dbContext.KanbanBoards
                .AsNoTracking()
                .Include(x => x.Columns).ThenInclude(x => x.Items)
                .FirstOrDefaultAsync(x => x.ID == query.BoardId);

            var result = _mapper.Map<KanbanBoardDTO>(board);

            return result;
        }

        public class Query : IQuery<KanbanBoardDTO>
        {
            public Query(int boardId)
            {
                BoardId = boardId;
            }

            public int BoardId { get; set; }
        }
    }
}
