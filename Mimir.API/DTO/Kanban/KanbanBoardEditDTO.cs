using System;

namespace Mimir.API.DTO
{
    public class KanbanBoardEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
    }
}