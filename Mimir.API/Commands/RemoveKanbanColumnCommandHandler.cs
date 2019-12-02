using System;
using System.Threading.Tasks;
using Mimir.API.Commands.Abstract;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class RemoveKanbanColumnCommandHandler : KanbanBoardCommandHandler<RemoveKanbanColumnCommandHandler.Command>
    {

        public RemoveKanbanColumnCommandHandler(IKanbanRepository repository, IKanbanAccessService accessService) : base(repository, accessService)
        {
        }

        public override async Task HandleAsync(Command command)
        {
            await base.HandleAsync(command);
            await _repository.RemoveColumnAsync(command.BoardId, command.ColumnId);
        }

        public class Command: KanbanBoardCommand
        {
            public Command() : base()
            {
            }

            public Command(int userId, int boardId, int columnId) : base(userId, boardId)
            {
                ColumnId = columnId;
            }

            public int ColumnId { get; }
        }
    }
}
