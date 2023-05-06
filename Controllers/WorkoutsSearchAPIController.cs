using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsSearchAPIController : ControllerBase
    {

        // API controller for searching workouts
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

        // GET: api/WorkoutsSearchAPI/WorkoutName
        [HttpGet("{WorkoutName}")]
        public async Task<ActionResult<IEnumerable<WorkoutsModel>>> GetWorkouts(string WorkoutName)
        {
            var workouts = await _context.Workouts.Where(w => w.Name.Contains(WorkoutName)).ToListAsync();

            if (workouts == null)
            {
                return NotFound();
            }

            return workouts;
        }


    }
}
