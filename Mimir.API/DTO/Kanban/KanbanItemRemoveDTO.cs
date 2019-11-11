using System;

namespace Mimir.API.DTO
{
    public class KanbanItemRemoveDTO
    {
        public int ItemId { get; set; }
        public int BoardId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
