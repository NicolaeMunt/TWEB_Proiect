using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Entities.Post;
using Domain.Interfaces;
using TWEB_Proiect.Filters;
using TWEB_Proiect.Models;

namespace TWEB_Proiect.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public PostController(IPostService postService, IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        // GET: /Post
        public ActionResult Index(int page = 1)
        {
            const int pageSize = 10;
            var posts = _postService.GetPosts(page, pageSize).ToList();

            var postViewModels = posts.Select(p => new PostViewModel
            {
                Id = p.Id,
                Title = p.Title,
                AuthorUsername = p.User?.Username ?? "[deleted]",
                AuthorId = p.UserId,
                CreatedDate = p.CreatedDate,
                ModifiedDate = p.ModifiedDate,
                Upvotes = p.Upvotes,
                Downvotes = p.Downvotes,
                CommentCount = p.Comments?.Count ?? 0,
                IsAuthor = Session["UserId"] != null && (int)Session["UserId"] == p.UserId
            }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPosts = postViewModels.Count;
            ViewBag.HasMorePages = postViewModels.Count == pageSize;

            return View(postViewModels);
        }

        // GET: /Post/Details/5
        public ActionResult Details(int id)
        {
            var post = _postService.GetPostById(id);
            if (post == null)
                return HttpNotFound();

            var comments = _postService.GetCommentsByPostId(id).ToList();

            int? currentUserId = Session["UserId"] as int?;

            var commentViewModels = MapCommentsToViewModels(comments, currentUserId);

            var postViewModel = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                AuthorUsername = post.User?.Username ?? "[deleted]",
                AuthorId = post.UserId,
                CreatedDate = post.CreatedDate,
                ModifiedDate = post.ModifiedDate,
                Upvotes = post.Upvotes,
                Downvotes = post.Downvotes,
                Comments = commentViewModels,
                IsAuthor = currentUserId.HasValue && currentUserId.Value == post.UserId
            };

            return View(postViewModel);
        }

        // GET: /Post/Create
        [AuthenticationFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public ActionResult Create(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = (int)Session["UserId"];

                var post = new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    UserId = userId
                };

                try
                {
                    int postId = _postService.CreatePost(post);
                    return RedirectToAction("Details", new { id = postId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating post: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: /Post/Edit/5
        [AuthenticationFilter]
        public ActionResult Edit(int id)
        {
            var post = _postService.GetPostById(id);
            if (post == null)
                return HttpNotFound();

            int currentUserId = (int)Session["UserId"];
            if (post.UserId != currentUserId)
                return new HttpUnauthorizedResult("You are not authorized to edit this post");

            var model = new CreatePostViewModel
            {
                Title = post.Title,
                Content = post.Content
            };

            return View(model);
        }

        // POST: /Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public ActionResult Edit(int id, CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                int currentUserId = (int)Session["UserId"];

                var post = new Post
                {
                    Id = id,
                    Title = model.Title,
                    Content = model.Content
                };

                try
                {
                    bool result = _postService.UpdatePost(post, currentUserId);
                    if (result)
                        return RedirectToAction("Details", new { id });
                    else
                        ModelState.AddModelError("", "Post not found or could not be updated");
                }
                catch (UnauthorizedAccessException)
                {
                    return new HttpUnauthorizedResult("You are not authorized to edit this post");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating post: {ex.Message}");
                }
            }

            return View(model);
        }

        // POST: /Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public ActionResult Delete(int id)
        {
            int currentUserId = (int)Session["UserId"];

            try
            {
                bool result = _postService.DeletePost(id, currentUserId);
                if (result)
                    return RedirectToAction("Index");
                else
                    TempData["ErrorMessage"] = "Post not found or could not be deleted";
            }
            catch (UnauthorizedAccessException)
            {
                return new HttpUnauthorizedResult("You are not authorized to delete this post");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting post: {ex.Message}";
            }

            return RedirectToAction("Details", new { id });
        }

        // POST: /Post/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public ActionResult AddComment(CreateCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                int currentUserId = (int)Session["UserId"];

                var comment = new Comment
                {
                    PostId = model.PostId,
                    ParentCommentId = model.ParentCommentId,
                    Content = model.Content,
                    UserId = currentUserId
                };

                try
                {
                    int commentId = _postService.CreateComment(comment);
                    return RedirectToAction("Details", new { id = model.PostId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error adding comment: {ex.Message}");
                }
            }

            // If we got this far, something failed
            return RedirectToAction("Details", new { id = model.PostId });
        }

        // POST: /Post/EditComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public ActionResult EditComment(int id, string content, int postId)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Comment content cannot be empty";
                return RedirectToAction("Details", new { id = postId });
            }

            int currentUserId = (int)Session["UserId"];

            var comment = new Comment
            {
                Id = id,
                Content = content
            };

            try
            {
                bool result = _postService.UpdateComment(comment, currentUserId);
                if (!result)
                    TempData["ErrorMessage"] = "Comment not found or could not be updated";
            }
            catch (UnauthorizedAccessException)
            {
                return new HttpUnauthorizedResult("You are not authorized to edit this comment");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating comment: {ex.Message}";
            }

            return RedirectToAction("Details", new { id = postId });
        }

        // POST: /Post/DeleteComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthenticationFilter]
        public ActionResult DeleteComment(int id, int postId)
        {
            int currentUserId = (int)Session["UserId"];

            try
            {
                bool result = _postService.DeleteComment(id, currentUserId);
                if (!result)
                    TempData["ErrorMessage"] = "Comment not found or could not be deleted";
            }
            catch (UnauthorizedAccessException)
            {
                return new HttpUnauthorizedResult("You are not authorized to delete this comment");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting comment: {ex.Message}";
            }

            return RedirectToAction("Details", new { id = postId });
        }

        // Helper method to recursively map comments to view models
        private List<CommentViewModel> MapCommentsToViewModels(IEnumerable<Comment> comments, int? currentUserId)
        {
            return comments.Select(c => new CommentViewModel
            {
                Id = c.Id,
                PostId = c.PostId,
                ParentCommentId = c.ParentCommentId,
                Content = c.Content,
                AuthorUsername = c.User?.Username ?? "[deleted]",
                AuthorId = c.UserId,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate,
                Upvotes = c.Upvotes,
                Downvotes = c.Downvotes,
                Replies = MapCommentsToViewModels(c.Replies, currentUserId),
                IsAuthor = currentUserId.HasValue && currentUserId.Value == c.UserId
            }).ToList();
        }
    }
}