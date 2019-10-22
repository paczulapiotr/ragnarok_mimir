using System;

namespace Mimir.API.DTO
{
    public class KanbanBoardDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
    }
}