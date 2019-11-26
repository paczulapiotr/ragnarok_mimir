using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.API.DTO;
using Mimir.Core.CommonExceptions;
using Mimir.CQRS.Queries;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Queries
{
    public class GetBoardParticipantsQueryHandler : IQueryHandler<IEnumerable<AppUserBasicResultDTO>, GetBoardParticipantsQueryHandler.Query>
    {
        private readonly MimirDbContext _dbContext;
        private readonly IKanbanAccessService _accessService;

        public GetBoardParticipantsQueryHandler(MimirDbContext dbContext, IKanbanAccessService accessService)
        {
            _dbContext = dbContext;
            _accessService = accessService;
        }

        public async Task<IEnumerable<AppUserBasicResultDTO>> HandleAsync(Query query)
        {
            if(!_accessService.HasAccess(query.UserId, query.BoardId))
            {
                throw new ForbiddenException();
            }

            return (await _dbContext.KanbanBoardAccess
                .Where(x => x.BoardID == query.BoardId)
                .Select(x => new { x.UserWithAccessID, x.UserWithAccess.Name })
                .ToListAsync())
                .Select(x => new AppUserBasicResultDTO { Id = x.UserWithAccessID, Name = x.Name });
        }

        public class Query: IQuery<IEnumerable<AppUserBasicResultDTO>>
        {
            public int UserId { get; set; }
            public int BoardId { get; set; }
        }
    }
}
