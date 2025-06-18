using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWEB_Proiect.Domain.Entities
{
     [Table("Users")]
     public class User
     {
          [Key]
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Id { get; set; }

          [Required]
          [StringLength(50)]
          public string Username { get; set; }

          [Required]
          [StringLength(100)]
          [EmailAddress]
          public string Email { get; set; }

          [Required]
          [StringLength(255)]
          public string Password { get; set; }

          [StringLength(50)]
          public string FirstName { get; set; } // Может быть NULL в БД

          [StringLength(50)]
          public string LastName { get; set; } // Может быть NULL в БД

          [Required]
          public DateTime CreatedDate { get; set; } = DateTime.Now;

          [Required]
          public bool IsActive { get; set; } = true;

          public DateTime? LoginTime { get; set; }
     }
}