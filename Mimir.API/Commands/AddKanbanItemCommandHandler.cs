using System;
using System.Threading.Tasks;
using Mimir.API.Commands.Abstract;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class AddKanbanItemCommandHandler: KanbanBoardCommandHandler<AddKanbanItemCommandHandler.Command>
    {
        public AddKanbanItemCommandHandler(IKanbanRepository repository, IKanbanAccessService accessService)
        : base(repository, accessService)
        {
        }

        public override async Task HandleAsync(Command command)
        {
            await base.HandleAsync(command);
            await _repository.AddItemAsync(command.BoardId, command.Name, command.ColumnId);
        }

        public class Command: KanbanBoardCommand
        {
            public Command() : base() { }
            public Command(int userId, int boardId, string name, int columnId) 
                : base(userId, boardId)
            {
                Name = name;
                ColumnId = columnId;
            }
            public string Name { get; }
            public int ColumnId { get; }
        }
    }
}
