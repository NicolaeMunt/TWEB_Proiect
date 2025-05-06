using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TWEB_Proiect.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }

    [Table("ULoginData")]
    public class ULoginData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LoginData { get; set; }
        public DateTime LoginTime { get; set; }
    }
}