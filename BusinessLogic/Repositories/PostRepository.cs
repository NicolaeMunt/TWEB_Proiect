using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Data;
using Domain.Entities.Post;
using Domain.Interfaces;

namespace BusinessLogic.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Post GetById(int id)
        {
            return _context.Posts
                .Include("User")
                .Include("Comments.User")
                .Include("Comments.Replies.User")
                .FirstOrDefault(p => p.Id == id && !p.IsDeleted);
        }

        public IEnumerable<Post> GetPosts(int page = 1, int pageSize = 10)
        {
            return _context.Posts
                .Include("User")
                .Include("Comments")
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<Post> GetPostsByUserId(int userId)
        {
            return _context.Posts
                .Include("Comments")
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();
        }

        public int CreatePost(Post post)
        {
            post.CreatedDate = DateTime.UtcNow;
            post.Upvotes = 0;
            post.Downvotes = 0;
            post.IsDeleted = false;

            _context.Posts.Add(post);
            _context.SaveChanges();
            return post.Id;
        }

        public bool UpdatePost(Post post)
        {
            var existingPost = _context.Posts.Find(post.Id);
            if (existingPost == null || existingPost.IsDeleted)
                return false;

            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.ModifiedDate = DateTime.UtcNow;

            return _context.SaveChanges() > 0;
        }

        public bool DeletePost(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
                return false;

            // Soft delete
            post.IsDeleted = true;
            return _context.SaveChanges() > 0;
        }

        public Comment GetCommentById(int id)
        {
            return _context.Comments
                .Include("User")
                .Include("Replies.User")
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }

        public IEnumerable<Comment> GetCommentsByPostId(int postId)
        {
            return _context.Comments
                .Include("User")
                .Include("Replies.User")
                .Where(c => c.PostId == postId && c.ParentCommentId == null && !c.IsDeleted)
                .OrderBy(c => c.CreatedDate)
                .ToList();
        }

        public int CreateComment(Comment comment)
        {
            comment.CreatedDate = DateTime.UtcNow;
            comment.Upvotes = 0;
            comment.Downvotes = 0;
            comment.IsDeleted = false;

            _context.Comments.Add(comment);
            _context.SaveChanges();
            return comment.Id;
        }

        public bool UpdateComment(Comment comment)
        {
            var existingComment = _context.Comments.Find(comment.Id);
            if (existingComment == null || existingComment.IsDeleted)
                return false;

            existingComment.Content = comment.Content;
            existingComment.ModifiedDate = DateTime.UtcNow;

            return _context.SaveChanges() > 0;
        }

        public bool DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null)
                return false;

            // Soft delete
            comment.IsDeleted = true;
            comment.Content = "[deleted]";
            return _context.SaveChanges() > 0;
        }
    }
}