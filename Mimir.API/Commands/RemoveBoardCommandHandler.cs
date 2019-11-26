using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.CQRS.Commands;
using Mimir.Database;

namespace Mimir.API.Commands
{
    public class RemoveBoardCommandHandler : ICommandHandler<RemoveBoardCommandHandler.Command>
    {
        private readonly MimirDbContext _dbContext;

        public RemoveBoardCommandHandler(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(Command command)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var board = _dbContext.KanbanBoards
                        .Include(x => x.UsersWithAccess)
                        .Where(x => x.OwnerID == command.UserId)
                        .Where(x => x.ID == command.BoardID).FirstOrDefault();
                    if (board == null)
                    {
                        throw new ArgumentException("Could not remove such a board");
                    }

                    _dbContext.RemoveRange(board.UsersWithAccess);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.Remove(board);
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Unknown error occured while removing board", ex);
                }
            }
        }

        public class Command : ICommand
        {
            public int UserId { get; set; }
            public int BoardID { get; set; }
        }
    }
}
