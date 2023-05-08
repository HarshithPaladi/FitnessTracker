using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitnessTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<FitnessUser> _userManager;
        

        public HomeController(ILogger<HomeController> logger,UserManager<FitnessUser> userManager)
        {
            _logger = logger;
            this._userManager = userManager;

        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(this.User);
            if (userId == null)
            {
                ViewBag.userName = "Guest";
                return View();
            }
            var user = _userManager.FindByIdAsync(userId).Result;
            var name = user?.FirstName + " " + user?.LastName ?? "Guest";
            ViewBag.userName = name;
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}