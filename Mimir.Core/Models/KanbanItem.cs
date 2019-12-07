using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Mimir.Core.Models
{
    [DebuggerDisplay("{Name}, ID: {ID}, Index: {Index}, ColumnID: {ColumnID}")]
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
        [ForeignKey(nameof(Assignee))]
        public int? AssigneeId { get; set; }
        public AppUser Assignee { get; set; }
        public AppUser CreatedBy { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
