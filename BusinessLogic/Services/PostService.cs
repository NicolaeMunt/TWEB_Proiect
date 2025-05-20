using System;
using System.Collections.Generic;
using Domain.Entities.Post;
using Domain.Interfaces;

namespace BusinessLogic.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public Post GetPostById(int id)
        {
            return _postRepository.GetById(id);
        }

        public IEnumerable<Post> GetPosts(int page = 1, int pageSize = 10)
        {
            return _postRepository.GetPosts(page, pageSize);
        }

        public IEnumerable<Post> GetPostsByUserId(int userId)
        {
            return _postRepository.GetPostsByUserId(userId);
        }

        public int CreatePost(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title))
                throw new ArgumentException("Post title is required");

            if (string.IsNullOrWhiteSpace(post.Content))
                throw new ArgumentException("Post content is required");

            return _postRepository.CreatePost(post);
        }

        public bool UpdatePost(Post post, int currentUserId)
        {
            var existingPost = _postRepository.GetById(post.Id);
            if (existingPost == null)
                return false;

            // Check if user is authorized to update this post
            if (existingPost.UserId != currentUserId)
                throw new UnauthorizedAccessException("You are not authorized to update this post");

            return _postRepository.UpdatePost(post);
        }

        public bool DeletePost(int id, int currentUserId)
        {
            var existingPost = _postRepository.GetById(id);
            if (existingPost == null)
                return false;

            // Check if user is authorized to delete this post
            if (existingPost.UserId != currentUserId)
                throw new UnauthorizedAccessException("You are not authorized to delete this post");

            return _postRepository.DeletePost(id);
        }

        public Comment GetCommentById(int id)
        {
            return _postRepository.GetCommentById(id);
        }

        public IEnumerable<Comment> GetCommentsByPostId(int postId)
        {
            return _postRepository.GetCommentsByPostId(postId);
        }

        public int CreateComment(Comment comment)
        {
            if (string.IsNullOrWhiteSpace(comment.Content))
                throw new ArgumentException("Comment content is required");

            return _postRepository.CreateComment(comment);
        }

        public bool UpdateComment(Comment comment, int currentUserId)
        {
            var existingComment = _postRepository.GetCommentById(comment.Id);
            if (existingComment == null)
                return false;

            // Check if user is authorized to update this comment
            if (existingComment.UserId != currentUserId)
                throw new UnauthorizedAccessException("You are not authorized to update this comment");

            return _postRepository.UpdateComment(comment);
        }

        public bool DeleteComment(int id, int currentUserId)
        {
            var existingComment = _postRepository.GetCommentById(id);
            if (existingComment == null)
                return false;

            // Check if user is authorized to delete this comment
            if (existingComment.UserId != currentUserId)
                throw new UnauthorizedAccessException("You are not authorized to delete this comment");

            return _postRepository.DeleteComment(id);
        }
    }
}