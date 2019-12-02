using System.Threading.Tasks;
using Mimir.API.Commands.Abstract;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class EditKanbanColumnCommandHandler : KanbanBoardCommandHandler<EditKanbanColumnCommandHandler.Command>
    {
        public EditKanbanColumnCommandHandler(IKanbanRepository repository, IKanbanAccessService accessService) : base(repository, accessService)
        {
        }

        public override async Task HandleAsync(Command command)
        {
            await base.HandleAsync(command);
            await _repository.EditColumnAsync(command.BoardId, command.ColumnId, command.Name);
        }

        public class Command : KanbanBoardCommand
        {
            public Command(int userId, int boardId, int columnId, string name)
                : base(userId, boardId)
            {
                ColumnId = columnId;
                Name = name;
            }

            public int ColumnId { get; }
            public string Name { get; set; }
        }
    }
}
