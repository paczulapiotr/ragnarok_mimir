﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimir.API.DTO
{
    public class KanbanBoardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public IEnumerable<KanbanColumnDTO> Columns { get; set; }
    }
}