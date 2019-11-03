using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimir.Core.Models
{
    public class KanbanItem : IIndexable
    {
        [Key]
        public int ID { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey(nameof(ColumnID))]
        public KanbanColumn Column { get; set; }
        public int ColumnID { get; set; }
        public AppUser Assignee { get; set; }
        public AppUser CreatedBy { get; set; }
    }
}
