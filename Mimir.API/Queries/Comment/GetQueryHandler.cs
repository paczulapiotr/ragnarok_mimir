using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Mimir.API.DTO.Comment.Result;
using Mimir.Core.CommonExceptions;
using Mimir.CQRS.Queries;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Queries.Comment
{
    public class GetQueryHandler : IQueryHandler<IEnumerable<CommentDTO>, GetQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IKanbanAccessService _accessService;

        public GetQueryHandler(MimirDbContext dbContext, 
            MapperConfiguration mapperConfiguration, 
            IKanbanAccessService accessService)
        {
            _dbContext = dbContext;
            _mapperConfiguration = mapperConfiguration;
            _accessService = accessService;
        }

        public async Task<IEnumerable<CommentDTO>> HandleAsync(Query query)
        {
            var item = _dbContext.KanbanItems.Include(x => x.Column).FirstOrDefault(x => x.ID == query.ItemId);
            if (item == null)
                throw new NotFoundException("Given item doesn't exist");

            if(!_accessService.HasAccess(query.UserId, item.Column.KanbanBoardID))
                throw new ForbiddenException();

            var comments = await _dbContext.KanbanItems.Where(x => x.ID == query.ItemId)
                .Include(x => x.Comments)
                .ThenInclude(x=>x.Author)
                .SelectMany(x=>x.Comments)
                .OrderBy(x=>x.CreatedOn)
                .ProjectTo<CommentDTO>(_mapperConfiguration)
                .ToListAsync();

            return WithOwnership(comments, query.UserId);
        }

        private IEnumerable<CommentDTO> WithOwnership(IEnumerable<CommentDTO> comments, int userId)
        {
            return comments.Select(x =>
            {
                if (x.AuthorId == userId)
                    x.IsOwner = true;
                return x;
            }).ToList();
        }

        public class Query : IQuery<IEnumerable<CommentDTO>>
        {
            public int ItemId { get; set; }
            public int UserId { get; set; }
        }
    }
}
