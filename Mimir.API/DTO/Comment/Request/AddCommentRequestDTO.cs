using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mimir.API.DTO.Comment.Request
{
    public class AddCommentRequestDTO
    {
        public int BoardId { get; set; }
        public int ItemId { get; set; }
        public string Content { get; set; }
    }
}
