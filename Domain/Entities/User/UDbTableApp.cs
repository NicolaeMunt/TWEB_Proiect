using System;

namespace Domain.Entities.User
{
    public class UDbTable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
}