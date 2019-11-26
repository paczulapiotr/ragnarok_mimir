using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Mimir.Core.Models
{
    [DebuggerDisplay("{Name}, ID: {ID}")]
    public class KanbanBoard : TimestampRecord
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<KanbanColumn> Columns { get; set; } = new List<KanbanColumn>();

        [ForeignKey(nameof(OwnerID))]
        public AppUser Owner { get; set; }

        public int OwnerID { get; set; }

        public List<KanbanBoardAccess> UsersWithAccess { get; set; } = new List<KanbanBoardAccess>();
    }
}
