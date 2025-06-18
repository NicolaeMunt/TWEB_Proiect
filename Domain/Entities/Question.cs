using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWEB_Proiect.Domain.Entities
{
     [Table("Questions")]
     public class Question
     {
          [Key]
          public int Id { get; set; }

          [Required]
          [StringLength(100)]
          public string Username { get; set; }

          [Required]
          [StringLength(255)]
          [EmailAddress]
          public string Email { get; set; }

          [Required]
          [StringLength(200)]
          public string Title { get; set; }

          [Required]
          [StringLength(100)]
          public string Category { get; set; }

          [Column(TypeName = "ntext")]
          public string Details { get; set; }

          [StringLength(500)]
          public string AttachmentPath { get; set; }

          public DateTime CreatedDate { get; set; }

          public int? UserId { get; set; }

          public int Views { get; set; } = 0;

          public int Answers { get; set; } = 0;

          public bool IsResolved { get; set; } = false;

          // Навигационные свойства
          [ForeignKey("UserId")]
          public virtual User User { get; set; }

          // Добавляем коллекцию ответов
          public virtual ICollection<Answer> QuestionAnswers { get; set; } = new List<Answer>();
     }
}