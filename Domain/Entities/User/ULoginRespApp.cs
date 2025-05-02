using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.User
{
    public class ULoginRespApp
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual UserApp User { get; set; }
    }
}
