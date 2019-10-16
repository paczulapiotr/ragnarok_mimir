using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimir.Core.Models
{
    public class KanbanBoard
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
