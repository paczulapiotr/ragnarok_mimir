using System;

namespace Mimir.API.DTO.Comment.Result
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EditedOn { get; set; }
        public bool IsOwner { get; set; }
    }
}
