using Mimir.CQRS.Commands;

namespace Mimir.API.Commands.Abstract
{
    public abstract class KanbanBoardCommand : ICommand
    {
        protected KanbanBoardCommand() { }
        protected KanbanBoardCommand(int userId, int boardId)
        {
            UserId = userId;
            BoardId = boardId;
        }

        public int UserId { get; set; }
        public int BoardId { get; set; }
    }
}
