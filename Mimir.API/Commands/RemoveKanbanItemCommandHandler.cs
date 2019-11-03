using System;
using System.Threading.Tasks;
using Mimir.API.Commands.Abstract;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class RemoveKanbanItemCommandHandler : KanbanBoardCommandHandler<RemoveKanbanItemCommandHandler.Command>
    {

        public RemoveKanbanItemCommandHandler(IKanbanRepository repository, IKanbanAccessService accessService): base(repository, accessService)
        {

        }

        public override async Task HandleAsync(Command command)
        {
            await base.HandleAsync(command);
            await _repository.RemoveItemAsync(command.BoardId, command.ItemId, command.Timestamp);
        }

        public class Command: KanbanBoardCommand
        {
            public Command() : base()
            {
            }

            public Command(int userId, int boardId, int itemId, DateTime timestamp) : base(userId, boardId)
            {
                ItemId = itemId;
                Timestamp = timestamp;
            }

            public int ItemId { get; }
            public DateTime Timestamp { get; }
        }
    }
}
