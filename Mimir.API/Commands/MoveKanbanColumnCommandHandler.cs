using System;
using System.Threading.Tasks;
using Mimir.API.Commands.Abstract;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class MoveKanbanColumnCommandHandler : KanbanBoardCommandHandler<MoveKanbanColumnCommandHandler.Command>
    {

        public MoveKanbanColumnCommandHandler(IKanbanRepository repository, IKanbanAccessService accessService)
            : base(repository, accessService)
        {
        }

        public override async Task HandleAsync(Command command)
        {
            await base.HandleAsync(command);
            await _repository.MoveColumnAsync(command.BoardId, command.ColumnId, command.Index, command.Timestamp);
        }

        public class Command: KanbanBoardCommand
        {
            public Command() : base()
            {
            }
            public Command(int userId, int boardId, int columnId, int index, DateTime timestamp) : base(userId, boardId)
            {
                ColumnId = columnId;
                Index = index;
                Timestamp = timestamp;
            }

            public int ColumnId { get; set; }
            public int Index { get; }
            public DateTime Timestamp { get; }
        }
    }
}
