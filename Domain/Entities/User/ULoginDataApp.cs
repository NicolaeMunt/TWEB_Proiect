using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.User
{
    public class ULoginDataApp
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime LastLogin { get; set; }

        // Navigation property
        public virtual UserApp User { get; set; }
    }
}
