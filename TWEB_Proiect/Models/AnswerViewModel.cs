using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TWEB_Proiect.Models
{
     public class AnswerViewModel
     {
          public int Id { get; set; }

          [Required(ErrorMessage = "Răspunsul este obligatoriu")]
          [Display(Name = "Răspunsul tău")]
          public string Content { get; set; }

          [Required]
          public int QuestionId { get; set; }

          public int UserId { get; set; }

          public string Username { get; set; }

          public string Email { get; set; }

          public DateTime CreatedDate { get; set; }

          public bool IsAccepted { get; set; }

          [Display(Name = "Attachment")]
          public HttpPostedFileBase Attachment { get; set; }

          public string AttachmentPath { get; set; }

          // Информация о вопросе для контекста
          public string QuestionTitle { get; set; }
          public string QuestionDetails { get; set; }
          public string QuestionAuthor { get; set; }
     }
}