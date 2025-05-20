using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Post;

namespace Domain.Interfaces
{
    public interface IPostService
    {
        Post GetPostById(int id);
        IEnumerable<Post> GetPosts(int page = 1, int pageSize = 10);
        IEnumerable<Post> GetPostsByUserId(int userId);
        int CreatePost(Post post);
        bool UpdatePost(Post post, int currentUserId);
        bool DeletePost(int id, int currentUserId);

        Comment GetCommentById(int id);
        IEnumerable<Comment> GetCommentsByPostId(int postId);
        int CreateComment(Comment comment);
        bool UpdateComment(Comment comment, int currentUserId);
        bool DeleteComment(int id, int currentUserId);
    }
}