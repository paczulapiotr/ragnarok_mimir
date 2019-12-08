using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimir.API.DTO.Comment.Request
{
    public class DeleteCommentRequestDTO
    {
        public int BoardId { get; set; }
        public int ItemId { get; set; }
        public int CommentId { get; set; }
    }
}
