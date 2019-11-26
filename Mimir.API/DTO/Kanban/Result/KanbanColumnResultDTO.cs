using System;
using System.Collections.Generic;

namespace Mimir.API.DTO
{
    public class KanbanColumnResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public IEnumerable<KanbanItemResultDTO> Items { get; set; }
    }
}