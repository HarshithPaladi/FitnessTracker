using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public class ChartController : Controller
    {
    private readonly UserManager<FitnessUser> _userManager;
        private readonly ApplicationDbContext _context;
        public ChartController(ApplicationDbContext context,UserManager<FitnessUser> userManager)
        {
            this._userManager = userManager;
            this._context = context;

        }
        [Authorize(Roles = "PremiumUser")]
        public IActionResult Index()
        {
            var currentUserId = _userManager.GetUserId(this.User);
            // pass vitals for current user to view
            var vitals = _context.Vitals.Where(v => v.FitnessUserId == currentUserId).ToList();
            return View(vitals);
        }
    }
}
