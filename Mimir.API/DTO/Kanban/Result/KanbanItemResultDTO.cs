using System;

namespace Mimir.API.DTO
{
    public class KanbanItemResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AssigneeName { get; set; }
        public int Index { get; set; }
    }
}