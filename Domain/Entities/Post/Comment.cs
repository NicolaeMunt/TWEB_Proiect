// Domain/Entities/Post/Comment.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Post
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(6_000)]
        public string Body { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // -------------- relationships ------------------------
        [Required]
        public int PostId { get; set; }
        public virtual ApplicationPost Post { get; set; }

        [Required]
        public string AuthorId { get; set; } = string.Empty;
    }
}
