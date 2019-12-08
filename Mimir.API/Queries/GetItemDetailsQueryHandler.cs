using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.CQRS.Queries;
using Mimir.Database;
using Mimir.Kanban;
using Mimir.Core.CommonExceptions;
using Mimir.API.DTO;
using AutoMapper;
using System.Collections.Generic;
using Mimir.API.DTO.Comment.Result;

namespace Mimir.API.Queries
{
    public class GetItemDetailsQueryHandler : IQueryHandler<KanbanItemDetailsResultDTO, GetItemDetailsQueryHandler.Query>
    {
        private readonly IKanbanAccessService _accessService;
        private readonly MimirDbContext _dbContext;
        private readonly MapperConfiguration _mapperFactory;

        public GetItemDetailsQueryHandler(IKanbanAccessService accessService, MimirDbContext dbContext, MapperConfiguration mapperFactory)
        {
            _accessService = accessService;
            _dbContext = dbContext;
            _mapperFactory = mapperFactory;
        }

        public async Task<KanbanItemDetailsResultDTO> HandleAsync(Query query)
        {
            var item = await _dbContext.KanbanItems
                .Include(x => x.Column)
                .Include(x => x.Comments).ThenInclude(x => x.Author)
                .Include(x => x.Assignee)
                .Where(x => x.ID == query.ItemId)
                .FirstOrDefaultAsync();

            if (item == null)
            {
                throw new NotFoundException("");
            }

            if (!_accessService.HasAccess(query.UserId, item.Column.KanbanBoardID))
            {
                throw new ForbiddenException();
            }

            var details = _mapperFactory.CreateMapper()
                .Map<KanbanItemDetailsResultDTO>(item);
            details.Comments = CommentsWithOwnership(details.Comments, query.UserId);

            return details;
        }

        private IEnumerable<CommentDTO> CommentsWithOwnership(IEnumerable<CommentDTO> comments, int userId)
        {
            return comments.Select(x =>
            {
                if (x.AuthorId == userId)
                    x.IsOwner = true;
                return x;
            }).ToList();
        }

        public class Query : IQuery<KanbanItemDetailsResultDTO>
        {
            public Query()
            {
            }

            public Query(int userId, int itemId)
            {
                UserId = userId;
                ItemId = itemId;
            }

            public int UserId { get; set; }
            public int ItemId { get; set; }
        }
    }
}
