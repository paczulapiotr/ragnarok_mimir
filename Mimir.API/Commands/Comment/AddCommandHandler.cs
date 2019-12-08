using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.API.Commands.Abstract;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Commands.Comment
{
    public class AddCommandHandler : KanbanBoardCommandHandler<AddCommandHandler.Command>
    {
        private readonly MimirDbContext _dbContext;

        public AddCommandHandler(IKanbanRepository repository, 
            IKanbanAccessService accessService,
            MimirDbContext dbContext) 
            : base(repository, accessService)
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
                .Where(x => x.ID == command.ItemId)
                .FirstOrDefault();

            item.Comments.Add(new Core.Models.Comment
            {
                CreatedOn = DateTime.Now,
                AuthorId = command.UserId,
                Content = command.Content,
            });

            await _dbContext.SaveChangesAsync();
        }

        public class Command : KanbanBoardCommand
        {
            public int ItemId { get; set; }
            public string Content { get; set; }
        }
    }
}
