﻿using System.Linq;
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
        private readonly UserManager<FitnessUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, UserManager<FitnessUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }


        public IActionResult Index()
        {
            // Get the count of all users except for the current user (admin)
            int userCount = _context.FitnessUsers.Count() - _userManager.GetUsersInRoleAsync("Admin").Result.Count();
            // Get the count of all users whose role is "PremiumUser"
            int premiumUserCount = _userManager.GetUsersInRoleAsync("PremiumUser").Result.Count();
            // Get a list of all users except for the current user (admin)
            var userList = _context.FitnessUsers
                .Where(u => u.Id != User.Identity.Name)
                .ToList();

            // Pass the user count and list of users to the view
            ViewData["UserCount"] = userCount;
            ViewData["PremiumUserCount"] = premiumUserCount;
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
[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // Handle error - user not found
            }

            // Remove all current roles from the user
            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!result.Succeeded)
            {
                // Handle error - failed to remove current roles
            }

            // Add the new role to the user
            result = await _userManager.AddToRoleAsync(user, newRole);

            if (!result.Succeeded)
            {
                // Handle error - failed to add new role
            }

            // Redirect to the user list page, or some other appropriate page
            return RedirectToAction(nameof(Index));
        }

    }
}
