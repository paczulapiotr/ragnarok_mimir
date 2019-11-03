namespace Mimir.Kanban
{
    public interface IKanbanAccessService
    {
        bool HasAccess(int userId, int boardId);

        bool IsOwner(int userId, int boardId);
    }
}