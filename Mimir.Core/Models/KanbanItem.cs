using System.ComponentModel.DataAnnotations;

namespace Mimir.Core.Models
{
    public class KanbanItem : TimestampRecord
    {
        [Key]
        public int ID { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AppUser Assignee { get; set; }
        public AppUser CreatedBy { get; set; }
    }
}
