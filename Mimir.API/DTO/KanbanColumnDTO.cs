using System;
using System.Collections.Generic;

namespace Mimir.API.DTO
{
    public class KanbanColumnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public IEnumerable<KanbanItemDTO> Items { get; set; }
    }
}