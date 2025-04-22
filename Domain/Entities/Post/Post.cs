using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.User;


namespace Domain.Entities.Post
{
    public class ApplicationPost
    {
        public int Id { get; set; }

        [Required, StringLength(300)]
        public string Title { get; set; } = String.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Body { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Ratings / votes
        public int Score { get; set; } = 0;

        // Navigation properties
        public int AuthorId { get; set; }
        public UserApplication Author { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
