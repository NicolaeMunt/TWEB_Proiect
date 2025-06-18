using System.ComponentModel.DataAnnotations;

namespace TWEB_Proiect.Models
{
     public class RegisterViewModel
     {
          [Required(ErrorMessage = "Numele de utilizator este obligatoriu.")]
          [Display(Name = "Username")]
          [StringLength(50, ErrorMessage = "Numele de utilizator trebuie să aibă între {2} și {1} caractere.", MinimumLength = 3)]
          [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Numele de utilizator poate conține doar litere, cifre și underscore.")]
          public string Username { get; set; }

          [Required(ErrorMessage = "Email-ul este obligatoriu.")]
          [EmailAddress(ErrorMessage = "Formatul email-ului nu este valid.")]
          [Display(Name = "Email")]
          [StringLength(100, ErrorMessage = "Email-ul nu poate depăși {1} caractere.")]
          public string Email { get; set; }

          [Required(ErrorMessage = "Parola este obligatorie.")]
          [StringLength(100, ErrorMessage = "Parola trebuie să aibă între {2} și {1} caractere.", MinimumLength = 6)]
          [DataType(DataType.Password)]
          [Display(Name = "Password")]
          public string Password { get; set; }

          [DataType(DataType.Password)]
          [Display(Name = "Confirm password")]
          [Compare("Password", ErrorMessage = "Parola și confirmarea parolei nu se potrivesc.")]
          public string ConfirmPassword { get; set; }
     }
}