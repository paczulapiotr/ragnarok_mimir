namespace Mimir.Kanban
{
    public interface IKanbanAccessService
    {
        bool HasAccess(int boardId);
        bool HasAccess(int userId, int boardId);

        bool IsOwner(int boardId);
        bool IsOwner(int userId, int boardId);
    }
}