// Domain/Entities/User/UserMinimal.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entities.Post;

namespace Domain.Entities.User
{
    public enum URole { Regular = 0, Moderator = 1, Admin = 2 }

    public class UserApplication
    {
        [Key]                      // PK
        public int Id { get; set; }

        [Required, StringLength(40)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(254)]
        public string Email { get; set; } = string.Empty;

        public DateTime LastLogin { get; set; } = DateTime.UtcNow;

        [StringLength(45)]         // IPv6 max length
        public string LastIp { get; set; } = string.Empty;

        [Required]
        public URole Level { get; set; } = URole.Regular;

        // ---- navigation ---------------------------------------------
        public virtual ICollection<ApplicationPost> Posts { get; set; }
            = new List<ApplicationPost>();
    }
}
