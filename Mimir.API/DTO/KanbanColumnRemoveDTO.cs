using System;

namespace Mimir.API.DTO
{
    public class KanbanColumnRemoveDTO
    {
        public int ColumnId { get; set; }
        public int BoardId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
