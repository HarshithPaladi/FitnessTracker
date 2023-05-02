using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace FitnessTracker.Controllers
{
    public class WorkoutsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<FitnessUser> _userManager;

        public WorkoutsController(ApplicationDbContext context, UserManager<FitnessUser> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        // GET: Workouts
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(this.User);

            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var applicationDbContext = _context.Workouts.Include(w => w.FitnessUser).Where(b => b.UserId==currentUserId);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workoutModel = await _context.Workouts
                .Include(w => w.FitnessUser)
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workoutModel == null)
            {
                return NotFound();
            }

            return View(workoutModel);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.FitnessUsers, "Id", "Id");
            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkoutId,Name,Type,Description,Date,Duration,CaloriesBurned,AverageHeartRate,BloodPressureSystolic,BloodPressureDiastolic,UserId,FitnessUser")] WorkoutModel workout)
        {
                var user = await _userManager.GetUserAsync(User);
                workout.UserId = user.Id; // set the UserId field of the new WorkoutModel instance
                workout.FitnessUser = user; // set the FitnessUser navigation property of the new WorkoutModel instance
            //if (ModelState.IsValid)
            //{
                _context.Add(workout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            if(!ModelState.IsValid)
            {
                // if the model state is not valid, return a bad request with the error message
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var workoutModel = workout.ToString();
                var errorMessage = string.Join(", ", errorMessages);
                var customMessage = string.Join(workoutModel, errorMessage);
                return BadRequest(customMessage);
            }
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("WorkoutId,Name,Type,Description,Date,Duration,CaloriesBurned,AverageHeartRate,BloodPressureSystolic,BloodPressureDiastolic,UserId")] WorkoutModel workoutModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(workoutModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserId"] = new SelectList(_context.FitnessUsers, "Id", "Id", workoutModel.UserId);
        //    return View(workoutModel);
        //}

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workoutModel = await _context.Workouts.FindAsync(id);
            if (workoutModel == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.FitnessUsers, "Id", "Id", workoutModel.UserId);
            return View(workoutModel);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkoutId,Name,Type,Description,Date,Duration,CaloriesBurned,AverageHeartRate,BloodPressureSystolic,BloodPressureDiastolic,UserId")] WorkoutModel workoutModel)
        {
            if (id != workoutModel.WorkoutId)
            {
                return NotFound();
            }


                try
                {
                    _context.Update(workoutModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutModelExists(workoutModel.WorkoutId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            ViewData["UserId"] = new SelectList(_context.FitnessUsers, "Id", "Id", workoutModel.UserId);
            return View(workoutModel);
        }

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workoutModel = await _context.Workouts
                .Include(w => w.FitnessUser)
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workoutModel == null)
            {
                return NotFound();
            }

            return View(workoutModel);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workouts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Workouts'  is null.");
            }
            var workoutModel = await _context.Workouts.FindAsync(id);
            if (workoutModel != null)
            {
                _context.Workouts.Remove(workoutModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutModelExists(int id)
        {
          return (_context.Workouts?.Any(e => e.WorkoutId == id)).GetValueOrDefault();
        }
    }
}
