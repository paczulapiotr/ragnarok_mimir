using System;
using System.Threading.Tasks;
using Mimir.API.Commands.Abstract;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class MoveKanbanItemCommandHandler : KanbanBoardCommandHandler<MoveKanbanItemCommandHandler.Command>
    {
        public MoveKanbanItemCommandHandler(IKanbanRepository repository, IKanbanAccessService accessService) : base (repository, accessService)
        {
        }

        public override async Task HandleAsync(Command command)
        {
            await base.HandleAsync(command);
            await _repository.MoveItemAsync(command.BoardId, command.ItemId, command.Index, command.DestColumnId, command.Timestamp);
        }


        public class Command : KanbanBoardCommand
        {
            public Command() : base() {}

            public Command(int userId, int boardId, int itemId, int index, int? destColumnId, DateTime timestamp) : base(userId, boardId)
            {
                ItemId = itemId;
                Index = index;
                DestColumnId = destColumnId;
                Timestamp = timestamp;
            }

            public int ItemId { get; }
            public int Index { get; }
            public int? DestColumnId { get; }
            public DateTime Timestamp { get; }
        }

    }
}
