using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.User;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Post
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public virtual Domain.Entities.User.User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
