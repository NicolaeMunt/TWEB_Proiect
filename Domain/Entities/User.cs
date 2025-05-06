using System;
using System.ComponentModel.DataAnnotations;

namespace TWEB_Proiect.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public DateTime LoginTime { get; set; }
    }
}