using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWEB_Proiect.Domain.Entities
{
     [Table("Answers")]
     public class Answer
     {
          [Key]
          public int Id { get; set; }

          [Required]
          [Column(TypeName = "ntext")]
          public string Content { get; set; }

          [Required]
          public int QuestionId { get; set; }

          [Required]
          public int UserId { get; set; }

          [Required]
          [StringLength(100)]
          public string Username { get; set; }

          [Required]
          [StringLength(255)]
          [EmailAddress]
          public string Email { get; set; }

          public DateTime CreatedDate { get; set; }

          public bool IsAccepted { get; set; } = false;

          [StringLength(500)]
          public string AttachmentPath { get; set; }

          // Навигационные свойства
          [ForeignKey("QuestionId")]
          public virtual Question Question { get; set; }

          [ForeignKey("UserId")]
          public virtual User User { get; set; }
     }
}