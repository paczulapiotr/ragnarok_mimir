using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimir.API.DTO
{
    public class KanbanItemAddDTO
    {
        public string Name { get; set; }
        public int BoardId { get; set; }
        public int ColumnId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
