using System;

namespace Mimir.API.DTO
{
    public class KanbanItemMoveDTO
    {
        public int ItemId { get; set; }
        public int? ColumnDestId { get; set; }
        public int IndexDest { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
