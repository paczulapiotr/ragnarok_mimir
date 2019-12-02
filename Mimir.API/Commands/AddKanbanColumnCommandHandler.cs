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
            await _repository.AddColumnAsync(command.BoardId, command.Name);
        }

        public class Command : KanbanBoardCommand
        {
            public Command()
                : base()
            { }

            public Command(int userId, int boardId, string name)
                : base(userId, boardId)
            {
                Name = name;
            }

            public string Name { get; }
        }
    }
}
