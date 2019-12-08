using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.API.Commands.Abstract;
using Mimir.Core.CommonExceptions;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Commands.Comment
{
    public class DeleteCommandHandler : KanbanBoardCommandHandler<DeleteCommandHandler.Command>
    {
        private readonly MimirDbContext _dbContext;

        public DeleteCommandHandler(
            IKanbanRepository repository, 
            IKanbanAccessService accessService, 
            MimirDbContext dbContext) : base(repository, accessService)
        {
            _dbContext = dbContext;
        }

        public override async Task HandleAsync(Command command)
        {
            await base.HandleAsync(command);
            var item = _dbContext.KanbanItems
                .Include(x => x.Column)
                .Include(x => x.Comments)
                .Where(x => x.Column.KanbanBoardID == command.BoardId)
                .FirstOrDefault(X => X.ID == command.ItemId);

            if (item == null)
                throw new NotFoundException("Given item was not found");

            var comment = item.Comments.FirstOrDefault(x => x.ID == command.CommentId);

            if (comment == null)
                throw new NotFoundException("Given comment was not found");

            _dbContext.Remove(comment);
            await _dbContext.SaveChangesAsync();
        }
        public class Command : KanbanBoardCommand
        {
            public int ItemId { get; set; }
            public int CommentId { get; set; }
        }
    }
}
