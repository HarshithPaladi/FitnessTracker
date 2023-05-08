using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsSearchAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutsSearchAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkoutsSearchAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutsModel>>> GetWorkouts()
        {
            return await _context.Workouts.ToListAsync();
        }

        // GET: api/WorkoutsSearchAPI/Search/{searchTerm}
        [Authorize]
        [HttpGet("Search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<WorkoutsModel>>> SearchWorkouts(string searchTerm)
        {
            var currentUserId = _context.Users.Where(u => u.UserName == User.Identity.Name)
                                    .Select(u => u.Id).FirstOrDefault();

            var workouts = await _context.Workouts
                .Where(w => EF.Functions.Like(w.Name, $"%{searchTerm}%")
                    || EF.Functions.Like(w.Type, $"%{searchTerm}%")
                    || EF.Functions.Like(w.Description, $"%{searchTerm}%")
                    || EF.Functions.Like(w.CaloriesBurned.ToString(), $"%{searchTerm}%")
                    || EF.Functions.Like(w.Vitals.HeartRate.ToString(), $"%{searchTerm}%")
                    || EF.Functions.Like(w.Vitals.DiastolicBP.ToString(), $"%{searchTerm}%")
                    || EF.Functions.Like(w.Vitals.SystolicBP.ToString(), $"%{searchTerm}%")
                    || EF.Functions.Like(w.Vitals.OxygenSaturation.ToString(), $"%{searchTerm}%"))
                    // fitnessuserid = currentUserId
                    .Where(w => w.FitnessUserId == currentUserId)
                .OrderByDescending(w => w.Date)
                .ToListAsync();

            if (!workouts.Any())
            {
                return NotFound();
            }

            return workouts;
        }
    }
}