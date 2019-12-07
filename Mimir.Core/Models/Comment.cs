using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimir.Core.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }

        public AppUser Author { get; set; }

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? EditedOn { get; set; }

    }
}
