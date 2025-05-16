// TWEB_Project/Controllers/AdminController.cs
using System;
using System.Web.Mvc;
using Domain.Interfaces;
using TWEB_Proiect.Filters;

namespace TWEB_Proiect.Controllers
{
    [AdminAuthorizationFilter] // We'll create this filter
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // Dashboard
        public ActionResult Dashboard()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        // Promote user to admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PromoteToAdmin(int id)
        {
            var result = _userService.PromoteToAdmin(id);
            if (result)
                TempData["SuccessMessage"] = "User promoted to admin successfully.";
            else
                TempData["ErrorMessage"] = "Failed to promote user.";

            return RedirectToAction("Dashboard");
        }

        // Demote user from admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DemoteFromAdmin(int id)
        {
            var result = _userService.DemoteFromAdmin(id);
            if (result)
                TempData["SuccessMessage"] = "User demoted from admin successfully.";
            else
                TempData["ErrorMessage"] = "Failed to demote user.";

            return RedirectToAction("Dashboard");
        }

        // Delete user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);
            if (result)
                TempData["SuccessMessage"] = "User deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete user.";

            return RedirectToAction("Dashboard");
        }
    }
}