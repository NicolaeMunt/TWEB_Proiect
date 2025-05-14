using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TWEB_Proiect.Models
{
    public class QuestionViewModel
    {
        [Required(ErrorMessage = "Numele utilizatorului obligatoriu")]
        [Display(Name = "Numele Utilizatorului")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email obligatoriu")]
        [EmailAddress(ErrorMessage = "Email incorect")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Titlul postului e obligatoriu")]
        [StringLength(200, ErrorMessage = "Titlul nu întrece limita de 200 de caractere")]
        [Display(Name = "Titlul întrebării")]
        public string QuestionTitle { get; set; }

        [Required(ErrorMessage = "Categoria e obligatorie")]
        [Display(Name = "Categorie")]
        public string Category { get; set; }

        [Display(Name = "Attachment")]
        public HttpPostedFileBase Attachment { get; set; }

        [Required(ErrorMessage = "Detaliile întrebării sunt obligatorii")]
        [Display(Name = "Detaliile întrebării")]
        public string QuestionDetails { get; set; }

        // Дополнительные поля для базы данных
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UserId { get; set; }
        public string AttachmentPath { get; set; }
        public int Views { get; set; }
        public int Answers { get; set; }
        public bool IsResolved { get; set; }
    }

    // Список категорий
    public static class QuestionCategories
    {
        public static readonly string[] Categories = {
            "Interese",
            "Muzică",
            "Diferite",
            "Ajutor",
            "Sesiune",
            "Părere",
            "Cinematograf",
            "Timp Liber",
            "Tehnologii",
            "Business"
        };
    }
}