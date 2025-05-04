using System;

namespace Domain.Entities.User
{
    public class ULoginResp
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}