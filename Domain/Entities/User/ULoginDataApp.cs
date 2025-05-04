using System;

namespace Domain.Entities.User
{
    public class ULoginData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime LastLogin { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}