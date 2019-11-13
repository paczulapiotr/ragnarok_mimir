using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.Core.CommonExceptions;
using Mimir.Core.Models;
using Mimir.CQRS.Commands;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class EditBoardParticipantsCommandHandler : ICommandHandler<EditBoardParticipantsCommandHandler.Command>
    {
        private readonly MimirDbContext _dbContext;
        private readonly IKanbanAccessService _accessService;

        public EditBoardParticipantsCommandHandler(MimirDbContext dbContext,
            IKanbanAccessService accessService)
        {
            _dbContext = dbContext;
            _accessService = accessService;
        }

        public async Task HandleAsync(Command command)
        {
            if (!_accessService.IsOwner(command.UserId, command.BoardId))
            {
                throw new ForbiddenException();
            }

            var newParticipants = _dbContext.AppUsers
                                            .Where(x => command.ParticipantIds.Contains(x.ID))
                                            .ToList();

            if (newParticipants.Count != command.ParticipantIds.Count())
                throw new ArgumentException("Invalid new participants");


            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var previousParticipants = _dbContext.KanbanBoardAccess
                        .Where(x => x.BoardID == command.BoardId)
                        .ToList();
                    _dbContext.RemoveRange(previousParticipants);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.AddRange(newParticipants.Select(x => 
                    new KanbanBoardAccess
                    {
                        BoardID = command.BoardId,
                        UserWithAccessID = x.ID
                    }));
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Unknown error occured while editing board participants", ex);
                }
            }
        }

        public class Command : ICommand
        {
            public int UserId { get; set; }
            public int BoardId { get; set; }
            public IEnumerable<int> ParticipantIds { get; set; }
        }
    }
}
