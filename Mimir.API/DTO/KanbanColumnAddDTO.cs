using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimir.API.DTO
{
    public class KanbanColumnAddDTO
    {
        public string Name { get; set; }
        public int BoardId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
