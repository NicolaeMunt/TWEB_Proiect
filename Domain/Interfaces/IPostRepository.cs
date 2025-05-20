using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Post;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        Post GetById(int id);
        IEnumerable<Post> GetPosts(int page = 1, int pageSize = 10);
        IEnumerable<Post> GetPostsByUserId(int userId);
        int CreatePost(Post post);
        bool UpdatePost(Post post);
        bool DeletePost(int id);

        Comment GetCommentById(int id);
        IEnumerable<Comment> GetCommentsByPostId(int postId);
        int CreateComment(Comment comment);
        bool UpdateComment(Comment comment);
        bool DeleteComment(int id);
    }
}