using System.Collections.Generic;
using Mimir.API.DTO.Comment.Result;

namespace Mimir.API.DTO
{
    public class KanbanItemDetailsResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AppUserBasicResultDTO Assignee { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}
