using System;

namespace Mimir.API.DTO
{
    public class KanbanItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
    }
}