using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
//using FitnessTracker.Migrations;
using System.Security.Claims;
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
            var workouts = await _context.Workouts
                .Where(w => w.FitnessUserId == currentUserId)
                .OrderByDescending(w => w.Date)
                .ToListAsync();

            return View(workouts);
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workoutsModel = await _context.Workouts
                .Include(w => w.Vitals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workoutsModel == null)
            {
                return NotFound();
            }

            return View(workoutsModel);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {
            var user = _userManager.GetUserAsync(User).Result;
            ViewBag.CurrentUserId = user.Id;
            var model = new WorkoutsModel
            {
                Vitals = new VitalsModel()
            };
            return View(model);
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Description,Date,Duration,CaloriesBurned,FitnessUserId,Vitals")]WorkoutsModel model, string FitnessUserId)
        {
            if (ModelState.IsValid)
            {
                // Create new workout object from view model data
                var workout = new WorkoutsModel
                {
                    Name = model.Name,
                    Type = model.Type,
                    Description = model.Description,
                    Date = model.Date,
                    Duration = model.Duration,
                    FitnessUserId = model.FitnessUserId
                };

                // Save workout to database
                _context.Workouts.Add(workout);
                await _context.SaveChangesAsync();

                // Create new vitals object if provided in view model
                if (model.Vitals.OxygenSaturation != null || model.Vitals.DiastolicBP!= null 
                    || model.Vitals.SystolicBP != null || model.Vitals.HeartRate != null)
                {
                    var vitals = new VitalsModel
                    {
                        OxygenSaturation = model.Vitals.OxygenSaturation,
                        DiastolicBP = model.Vitals.DiastolicBP,
                        SystolicBP = model.Vitals.SystolicBP,
                        HeartRate = model.Vitals.HeartRate,
                        FitnessUserId = model.Vitals.FitnessUserId
                    };

                    // Save vitals to database
                    _context.Vitals.Add(vitals);
                    await _context.SaveChangesAsync();

                    // set the vitalsId in the WorkoutsModel to reference the new VitalsModel
                    workout.VitalsId = vitals.VitalsId;

                    // update the workouts table
                    _context.Workouts.Update(workout);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                // if the model state is not valid, return a bad request with the error message
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var workoutModel = model.ToString();
                var errorMessage = string.Join(", ", errorMessages);
                var customMessage = string.Join(workoutModel, errorMessage);
                return BadRequest(customMessage);
            }

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Type,Description,Date,Duration,CaloriesBurned,FitnessUserId,Vitals")] WorkoutsModel workout)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Get the current user
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        var user = await _context.Users.FindAsync(userId);
        //        // Set the fitness user ID
        //        workout.FitnessUserId = user.Id;

        //        // Add the vitals data to the workout
        //        if (workout.Vitals != null)
        //        {
        //            workout.Vitals.Workout = workout;
        //            _context.Add(workout.Vitals);
        //        }

        //        _context.Add(workout);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(workout);
        //}

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workoutsModel = await _context.Workouts.FindAsync(id);
            if (workoutsModel == null)
            {
                return NotFound();
            }
            return View(workoutsModel);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Description,Date,Duration,CaloriesBurned,FitnessUserId")] WorkoutsModel workoutsModel)
        {
            if (id != workoutsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workoutsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutsModelExists(workoutsModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workoutsModel);
        }

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workoutsModel = await _context.Workouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workoutsModel == null)
            {
                return NotFound();
            }

            return View(workoutsModel);
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
            var workoutsModel = await _context.Workouts.FindAsync(id);
            if (workoutsModel != null)
            {
                _context.Workouts.Remove(workoutsModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutsModelExists(int id)
        {
          return (_context.Workouts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
