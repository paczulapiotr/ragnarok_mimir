using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mimir.API.Commands.Abstract;
using Mimir.CQRS.Commands;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class EditKanbanItemCommandHandler : KanbanBoardCommandHandler<EditKanbanItemCommandHandler.Command>
    {
        public EditKanbanItemCommandHandler(IKanbanRepository repository, IKanbanAccessService accessService) : base(repository, accessService)
        {
        }

        public override async Task HandleAsync(Command command)
        {
            await base.HandleAsync(command);
            await _repository.EditItemAsync(command.BoardId, command.ItemId, command.Name, command.Description, command.AssigneeId);
        }

        public class Command : KanbanBoardCommand
        {
            public Command(int userId, int boardId, int itemId, string name, string description, int? assigneeId) 
                : base(userId, boardId)
            {
                ItemId = itemId;
                Name = name;
                Description = description;
                AssigneeId = assigneeId;
            }

            public int ItemId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int? AssigneeId { get; set; }
        }
    }
}
