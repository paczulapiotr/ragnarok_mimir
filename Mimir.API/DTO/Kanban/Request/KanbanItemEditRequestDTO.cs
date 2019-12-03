namespace Mimir.API.DTO
{
    public class KanbanItemEditRequestDTO
    {
        public int BoardId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AssigneeId { get; set; }
    }
}
