using System;
using System.Threading.Tasks;

namespace Mimir.Kanban
{
    public interface IKanbanRepository
    {
        Task MoveItemAsync(int boardId, int itemId, int index, int? destColumnId, DateTime timestamp);
        Task MoveColumnAsync(int boardId, int columnId, int index, DateTime timestamp);
        Task AddItemAsync(int boardId, string name, int columnId, DateTime timestamp);
        Task AddColumnAsync(int boardId, string name, DateTime timestamp);
        Task RemoveItemAsync(int boardId, int itemId, DateTime timestamp);
        Task RemoveColumnAsync(int boardId, int columnId, DateTime timestamp);
    }
}
