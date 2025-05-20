using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWEB_Proiect.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        public string AuthorUsername { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int CommentCount { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public bool IsAuthor { get; set; }
    }

    public class CreatePostViewModel
    {
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }
    }

    public class CommentViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string Content { get; set; }

        public string AuthorUsername { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public List<CommentViewModel> Replies { get; set; } = new List<CommentViewModel>();
        public bool IsAuthor { get; set; }
    }

    public class CreateCommentViewModel
    {
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string Content { get; set; }
    }
}