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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace FitnessTracker.Controllers
{
    public class WorkoutsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<FitnessUser> _userManager;
        private readonly WorkoutsSearchAPIController _workoutsSearchAPI;

        public WorkoutsController(ApplicationDbContext context, UserManager<FitnessUser> userManager, WorkoutsSearchAPIController workoutsSearchAPI)
        {
            _context = context;
            this._userManager = userManager;
            this._workoutsSearchAPI = workoutsSearchAPI;
            // Get the user you want to add a role to
            var user = _userManager.FindByEmailAsync("test@example.com").Result;
            // Add the role to the user
            _userManager.AddToRoleAsync(user, "PremiumUser").Wait();
        }

        // GET: Workouts
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public IActionResult Create()
        {
            var user = _userManager.GetUserAsync(User).Result;
            ViewBag.CurrentUserId = user.Id;
            var model = new WorkoutsModel
            {
                Date = DateTime.Now
            };
            return View(model);
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Description,Date,Duration,CaloriesBurned,FitnessUserId,Vitals")] WorkoutsModel model)
        {
            var currentUserId = _userManager.GetUserId(this.User);
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
                    CaloriesBurned = model.CaloriesBurned,
                    FitnessUserId = currentUserId,
                };

                // Save workout to database
                _context.Workouts.Add(workout);
                await _context.SaveChangesAsync();

                // Create new vitals object if provided in view model
                if (model.Vitals != null && (model.Vitals.OxygenSaturation != null || model.Vitals.DiastolicBP != null
                    || model.Vitals.SystolicBP != null || model.Vitals.HeartRate != null))
                {
                    var vitals = new VitalsModel
                    {
                        OxygenSaturation = model.Vitals.OxygenSaturation,
                        DiastolicBP = model.Vitals.DiastolicBP,
                        SystolicBP = model.Vitals.SystolicBP,
                        HeartRate = model.Vitals.HeartRate,
                        FitnessUserId = currentUserId,
                        Date = model.Vitals.Date,
                    };

                    // Save vitals to database
                    _context.Vitals.Add(vitals);
                    await _context.SaveChangesAsync();

                    // set the vitalsId in the WorkoutsModel to reference the new VitalsModel
                    workout.VitalsId = vitals.VitalsId;
                    // set the workoutId in the VitalsModel to reference the new WorkoutsModel
                    vitals.WorkoutsId = workout.Id;

                    // update the workouts table
                    _context.Workouts.Update(workout);
                    // update the vitals table
                    _context.Vitals.Update(vitals);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            // if the model state is not valid, return a bad request with the error message
            var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            var workoutModel = model.ToString();
            var errorMessage = string.Join(", ", errorMessages);
            var customMessage = string.Join(workoutModel, errorMessage);
            return BadRequest(customMessage);
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
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            ViewBag.CurrentUserId = user.Id;
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workoutsModel = await _context.Workouts
                                    .Include(w => w.Vitals)
                                    .Where(w => w.FitnessUserId == user.Id)
                                    .FirstOrDefaultAsync(m => m.Id == id);
            // workoutsModel.Vitals = await _context.Vitals.FindAsync(workoutsModel.VitalsId).Result;
            // // Pass the vitals data to the view
            var vitals = await _context.Vitals.FindAsync(workoutsModel.VitalsId);
            ViewBag.Vitals = vitals;
            // var viewModel = new WorkoutsModel
            // {
            //     Vitals= vitals,
            //     Id = workoutsModel.Id,
            //     Name = workoutsModel.Name,
            //     Type = workoutsModel.Type,
            //     Description = workoutsModel.Description,
            //     Date = workoutsModel.Date,
            //     Duration = workoutsModel.Duration,
            //     CaloriesBurned = workoutsModel.CaloriesBurned,
            //     FitnessUserId = workoutsModel.FitnessUserId,
            // };


            if (workoutsModel == null)
            {
                return NotFound();
            }
            return View(workoutsModel);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,CustomType,Description,Date,Duration,CaloriesBurned,FitnessUserId,VitalsId,Vitals")] WorkoutsModel workoutsModel)
        {
            if (id != workoutsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // retrieve the Vitals object from the database based on the VitalsId
                    var vitals = await _context.Vitals.FindAsync(workoutsModel.VitalsId);

                    // update the Vitals object with the values from the form if the Vitals object exists
                    if (vitals != null)
                    {
                        await TryUpdateModelAsync(vitals);
                    }
                    // update the WorkoutsModel object with the values from the form
                    _context.Update(workoutsModel);

                    // save changes to the database
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

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Description,Date,Duration,CaloriesBurned,FitnessUserId,Vitals,VitalsId")] WorkoutsModel workoutsModel)
        // {
        //     var user = await _userManager.GetUserAsync(User);
        //     ViewBag.CurrentUserId = user.Id;
        //     if (id != workoutsModel.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             var existingWorkout = await _context.Workouts
        //                                 .Include(w => w.Vitals)
        //                                 .Include(w => w.VitalsId)
        //                                 .FirstOrDefaultAsync(w => w.Id == id && w.FitnessUserId == user.Id);

        //             if (existingWorkout != null)
        //             {
        //                 existingWorkout.Name = workoutsModel.Name;
        //                 existingWorkout.Type = workoutsModel.Type;
        //                 existingWorkout.Description = workoutsModel.Description;
        //                 existingWorkout.Date = workoutsModel.Date;
        //                 existingWorkout.Duration = workoutsModel.Duration;
        //                 existingWorkout.CaloriesBurned = workoutsModel.CaloriesBurned;

        //                 if (existingWorkout.Vitals == null)
        //                 {
        //                     existingWorkout.Vitals = new VitalsModel();
        //                 }
        //                 existingWorkout.VitalsId = workoutsModel.VitalsId;
        //                 existingWorkout.Vitals.HeartRate = workoutsModel.Vitals.HeartRate;
        //                 existingWorkout.Vitals.DiastolicBP = workoutsModel.Vitals.DiastolicBP;
        //                 existingWorkout.Vitals.SystolicBP = workoutsModel.Vitals.SystolicBP;
        //                 existingWorkout.Vitals.OxygenSaturation = workoutsModel.Vitals.OxygenSaturation;
        //                 existingWorkout.Vitals.Date = workoutsModel.Vitals.Date;

        //                 existingWorkout.Vitals.FitnessUserId = workoutsModel.Vitals.FitnessUserId;

        //                 _context.Update(existingWorkout);
        //                 await _context.SaveChangesAsync();
        //                 // workoutsModel.FitnessUserId = ViewBag.CurrentUserId;
        //                 // _context.Update(workoutsModel);
        //                 // await _context.SaveChangesAsync();
        //             }
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!WorkoutsModelExists(workoutsModel.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(workoutsModel);
        // }

        // GET: Workouts/Delete/5
        [Authorize]
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workouts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Workouts'  is null.");
            }
            var workoutsModel = await _context.Workouts.FindAsync(id);
            var vitalsModel = await _context.Vitals.FindAsync(workoutsModel.VitalsId);
            if (workoutsModel != null)
            {
                _context.Workouts.Remove(workoutsModel);
            }
            if (vitalsModel != null)
            {
                _context.Vitals.Remove(vitalsModel);
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