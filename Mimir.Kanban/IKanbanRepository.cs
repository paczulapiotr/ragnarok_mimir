using System;

namespace Mimir.Kanban
{
    public interface IKanbanRepository
    {
        bool MoveItem(int boardId, int itemId, int index, int? destColumnId, DateTime timestamp);
        bool MoveColumn(int boardId, int columnId, int index, DateTime timestamp);
        bool AddItem(int boardId, string name, int columnId, DateTime timestamp);
        bool AddColumn(int boardId, string name, DateTime timestamp);
        void RemoveItem(int boardId, int itemId, DateTime timestamp);
        void RemoveColumn(int boardId, int columnId, DateTime timestamp);
    }
}
