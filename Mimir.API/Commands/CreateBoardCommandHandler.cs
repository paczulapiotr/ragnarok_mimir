using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mimir.Core.Models;
using Mimir.CQRS.Commands;
using Mimir.Database;

namespace Mimir.API.Commands
{
    public class CreateBoardCommandHandler : ICommandHandler<CreateBoardCommandHandler.Command>
    {
        private readonly MimirDbContext _dbContext;

        public CreateBoardCommandHandler(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(Command command)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var board = new KanbanBoard
                    {
                        Name = command.Name,
                        OwnerID = command.UserId,
                    };

                    _dbContext.KanbanBoards.Add(board);
                    await _dbContext.SaveChangesAsync();

                    if (command.ParticipantIds != null && command.ParticipantIds.Any())
                    {
                        var participants = command.ParticipantIds.ToHashSet();
                        participants.Remove(command.UserId);
                        var boardAccess = participants.Select(x => new KanbanBoardAccess
                        {
                            BoardID = board.ID,
                            UserWithAccessID = x
                        }).ToList();

                        _dbContext.KanbanBoardAccess.AddRange(boardAccess);
                    }

                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public class Command : ICommand
        {
            public int UserId { get; set; }
            public string Name { get; set; }
            public  IEnumerable<int> ParticipantIds { get; set; }
        }
    }
}
