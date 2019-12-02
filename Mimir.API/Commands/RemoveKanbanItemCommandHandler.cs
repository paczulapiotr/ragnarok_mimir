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
            await _repository.RemoveItemAsync(command.BoardId, command.ItemId);
        }

        public class Command: KanbanBoardCommand
        {
            public Command() : base()
            {
            }

            public Command(int userId, int boardId, int itemId) : base(userId, boardId)
            {
                ItemId = itemId;
            }

            public int ItemId { get; }
        }
    }
}
