using System;
using System.Collections.Generic;
using System.Linq;
namespace Mimir.API.DTO
{
    public class KanbanColumnEditRequestDTO
    {
        public int ColumnId { get; set; }
        public int BoardId { get; set; }
        public string Name { get; set; }
    }
}
