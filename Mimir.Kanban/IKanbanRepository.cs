using System;
using System.Threading.Tasks;

namespace Mimir.Kanban
{
    public interface IKanbanRepository
    {
        Task MoveItemAsync(int boardId, int itemId, int index, int? destColumnId, DateTime timestamp);
        Task MoveColumnAsync(int boardId, int columnId, int index, DateTime timestamp);
        Task AddItemAsync(int boardId, string name, int columnId);
        Task AddColumnAsync(int boardId, string name);
        Task RemoveItemAsync(int boardId, int itemId);
        Task RemoveColumnAsync(int boardId, int columnId);
        Task EditColumnAsync(int boardId, int columnId, string name);
        Task EditItemAsync(int boardId, int itemId, string name, string description, int? assigneeId);
    }
}
