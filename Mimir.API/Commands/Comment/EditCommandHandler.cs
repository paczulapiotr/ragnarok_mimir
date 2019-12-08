using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mimir.API.Commands.Abstract;
using Mimir.Core.CommonExceptions;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Commands.Comment
{
    public class EditCommandHandler : KanbanBoardCommandHandler<EditCommandHandler.Command>
    {
        private readonly MimirDbContext _dbContext;

        public EditCommandHandler(
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
            comment.Content = command.Content;
            comment.EditedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }
        public class Command : KanbanBoardCommand
        {
            public int CommentId { get; set; }
            public string Content { get; set; }
            public int ItemId { get; set; }
        }
    }
}
