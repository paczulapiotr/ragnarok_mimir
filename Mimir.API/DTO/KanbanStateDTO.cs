using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimir.API.DTO
{
    public class KanbanStateDTO
    {
        public IEnumerable<KanbanColumnDTO> Columns { get; set; }
        public KanbanBoardDTO Board { get; set; }
    }
}
