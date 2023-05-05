using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkoutsAPI/searchString
        [HttpGet("{searchString}")]
        public IEnumerable<WorkoutsModel> Get(string searchString)
        {
            // Use fuzzy search to find workouts that match the search string
            var workouts = _context.Workouts
                .Where(w => w.Name.ToLower().Contains(searchString.ToLower()))
                .ToList();

            return workouts;
        }
    // GET: api/WorkoutsAPI
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutsModel>>> GetWorkouts()
    {
        if (_context.Workouts == null)
        {
            return NotFound();
        }
        return await _context.Workouts.ToListAsync();
    }

    // GET: api/WorkoutsAPI/5
    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutsModel>> GetWorkoutsModel(int id)
    {
        if (_context.Workouts == null)
        {
            return NotFound();
        }
        var workoutsModel = await _context.Workouts.FindAsync(id);

        if (workoutsModel == null)
        {
            return NotFound();
        }

        return workoutsModel;
    }

    // PUT: api/WorkoutsAPI/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutWorkoutsModel(int id, WorkoutsModel workoutsModel)
    {
        if (id != workoutsModel.Id)
        {
            return BadRequest();
        }

        _context.Entry(workoutsModel).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WorkoutsModelExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/WorkoutsAPI
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<WorkoutsModel>> PostWorkoutsModel(WorkoutsModel workoutsModel)
    {
        if (_context.Workouts == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Workouts'  is null.");
        }
        _context.Workouts.Add(workoutsModel);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetWorkoutsModel", new { id = workoutsModel.Id }, workoutsModel);
    }

    // DELETE: api/WorkoutsAPI/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkoutsModel(int id)
    {
        if (_context.Workouts == null)
        {
            return NotFound();
        }
        var workoutsModel = await _context.Workouts.FindAsync(id);
        if (workoutsModel == null)
        {
            return NotFound();
        }

        _context.Workouts.Remove(workoutsModel);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool WorkoutsModelExists(int id)
    {
        return (_context.Workouts?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
}
