using System;
using System.Threading.Tasks;
using Mimir.API.Commands.Abstract;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class AddKanbanColumnCommandHandler : KanbanBoardCommandHandler<AddKanbanColumnCommandHandler.Command>
    {
        public AddKanbanColumnCommandHandler(IKanbanRepository repository, IKanbanAccessService accessService)
        : base(repository, accessService)
        {
        }

        public override async Task HandleAsync(Command command)
        {
            await base.HandleAsync(command);
            await _repository.AddColumnAsync(command.BoardId, command.Name, command.Timestamp);
        }

        public class Command : KanbanBoardCommand
        {
            public Command()
                : base()
            { }

            public Command(int userId, int boardId, string name, DateTime timestamp)
                : base(userId, boardId)
            {
                Name = name;
                Timestamp = timestamp;
            }

            public string Name { get; }
            public DateTime Timestamp { get; }
        }
    }
}
