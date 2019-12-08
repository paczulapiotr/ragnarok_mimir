using System;
using System.Collections.Generic;

namespace Mimir.API.DTO
{
    public class KanbanBoardResultDTO
    {
        public int Id { get; set; }
        public bool IsOwner { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public IEnumerable<KanbanColumnResultDTO> Columns { get; set; }
    }
}
