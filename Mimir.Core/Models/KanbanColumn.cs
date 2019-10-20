using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mimir.Core.Models
{
    public class KanbanColumn : TimestampRecord
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public List<KanbanItem> Items { get; set; } = new List<KanbanItem>();
    }
}
