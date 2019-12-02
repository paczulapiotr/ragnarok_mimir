using System;

namespace Mimir.API.DTO
{
    public class KanbanItemAddRequestDTO
    {
        public string Name { get; set; }
        public int BoardId { get; set; }
        public int ColumnId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
