using System;

namespace Mimir.API.DTO
{
    public class KanbanColumnAddRequestDTO
    {
        public string Name { get; set; }
        public int BoardId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
