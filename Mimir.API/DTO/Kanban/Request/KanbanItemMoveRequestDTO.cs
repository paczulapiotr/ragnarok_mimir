using System;

namespace Mimir.API.DTO
{
    public class KanbanItemMoveRequestDTO
    {
        public int BoardId { get; set; }
        public int ItemId { get; set; }
        public int? ColumnDestId { get; set; }
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
