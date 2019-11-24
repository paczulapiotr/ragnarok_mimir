using System;

namespace Mimir.API.DTO
{
    public class KanbanColumnMoveRequestDTO
    {
        public int ColumnId { get; set; }
        public int Index { get; set; }
        public int BoardId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
