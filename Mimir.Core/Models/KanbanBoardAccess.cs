using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mimir.Core.Models
{
    public class KanbanBoardAccess
    {
        public int UserWithAccessID { get; set; }
        public AppUser UserWithAccess { get; set; }
        public int BoardID { get; set; }
        public KanbanBoard Board { get; set; }
    }
}
