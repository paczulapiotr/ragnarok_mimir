using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mimir.Core.Models
{
    public class AppUser
    {
        [Key]
        public int ID { get; set; }
        public string AuthID { get; set; }
        public string Name { get; set; }
        public List<KanbanBoard> OwnedBoards { get; set; } = new List<KanbanBoard>();
        public List<KanbanBoardAccess> BoardsWithAccess { get; set; } = new List<KanbanBoardAccess>();
    }
}
