using System.Linq;
using FitnessTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http.Extensions;



namespace FitnessTracker.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get the count of all users except for the current user (admin)
            int userCount = _context.FitnessUsers.Count() - 1;

            // Get a list of all users except for the current user (admin)
            var userList = _context.FitnessUsers
                .Where(u => u.Id != User.Identity.Name)
                .ToList();

            // Pass the user count and list of users to the view
            ViewData["UserCount"] = userCount;
            return View(userList);
        }
        // POST: Admin/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult DeleteUser(string id)
        {
            var user = _context.FitnessUsers.Find(id);
            if (user != null)
            {
                _context.FitnessUsers.Remove(user);
                _context.SaveChanges();
                TempData["SuccessMessage"] = $"User {user.UserName} has been deleted.";
            }
            else
            {
                TempData["ErrorMessage"] = $"User with id {id} not found.";
            }

            return RedirectToAction(nameof(Index));
        }
        // POST: Admin/ResetPassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(string id)
        {
            var user = _context.FitnessUsers.Find(id);
            if (user != null)
            {
                var newPassword = HttpContext.Request.Query["newPassword"];
                // hash the password
                var passwordHasher = new PasswordHasher<FitnessUser>();
                var passwordHash = passwordHasher.HashPassword(user, newPassword);
                user.PasswordHash = passwordHash;
                _context.SaveChanges();
                TempData["SuccessMessage"] = $"Password for user {user.UserName} has been reset.";
            }
            else
            {
                TempData["ErrorMessage"] = $"User with id {id} not found.";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
