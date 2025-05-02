using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.User
{
    public class UDbTableApp
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual UserApp User { get; set; }
    }
}
