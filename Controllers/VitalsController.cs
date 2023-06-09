﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FitnessTracker.Controllers
{
    public class VitalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<FitnessUser> _userManager;

        public VitalsController(ApplicationDbContext context, UserManager<FitnessUser> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        // GET: Vitals
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(this.User);

            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var vitals = await _context.Vitals
                .Where(w => w.FitnessUserId == currentUserId)
                .OrderByDescending(w => w.Date)
                .ToListAsync();

            return View(vitals);
        }

        // GET: Vitals/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vitals == null)
            {
                return NotFound();
            }

            var vitalsModel = await _context.Vitals
                .FirstOrDefaultAsync(m => m.VitalsId == id);
            if (vitalsModel == null)
            {
                return NotFound();
            }

            return View(vitalsModel);
        }

        // GET: Vitals/Create
        [Authorize]
        public IActionResult Create()
        {
            var user = _userManager.GetUserAsync(User).Result;
            ViewBag.CurrentUserId = user.Id;

            return View();
        }

        // POST: Vitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VitalsId,Date,HeartRate,SystolicBP,DiastolicBP,OxygenSaturation,FitnessUserId")] VitalsModel vitalsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vitalsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vitalsModel);
        }

        // Removed Edit methods due to vital signs being a one-time entry

        // // GET: Vitals/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     var user = _userManager.GetUserAsync(User).Result;
        //     ViewBag.CurrentUserId = user.Id;
        //     if (id == null || _context.Vitals == null)
        //     {
        //         return NotFound();
        //     }

        //     var vitalsModel = await _context.Vitals.FindAsync(id);
        //     if (vitalsModel == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(vitalsModel);
        // }

        // // POST: Vitals/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("VitalsId,Date,HeartRate,SystolicBP,DiastolicBP,OxygenSaturation,FitnessUserId")] VitalsModel vitalsModel)
        // {
        //     if (id != vitalsModel.VitalsId)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(vitalsModel);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!VitalsModelExists(vitalsModel.VitalsId))
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
        //     return View(vitalsModel);
        // }

        // GET: Vitals/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var currentUserId = _userManager.GetUserId(this.User);
            if (id == null || _context.Vitals == null)
            {
                return NotFound();
            }

            var vitalsModel = await _context.Vitals
            .Where(w => w.FitnessUserId == currentUserId)
                .FirstOrDefaultAsync(m => m.VitalsId == id);
            // Do not delete if workoutId is present
            if (vitalsModel.WorkoutsId != null)
            {
                // Add error message to TempData to display on the DeleteConfirmation page
                TempData["ErrorMessage"] = "This Vitals record cannot be deleted because it is associated with a Workout. Please delete the associated Workout first.";
                return RedirectToAction(nameof(Index), new { id = vitalsModel.VitalsId });
            }
            if (vitalsModel == null)
            {
                return NotFound();
            }

            return View(vitalsModel);
        }

        // POST: Vitals/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUserId = _userManager.GetUserId(this.User);
            if (_context.Vitals == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vitals'  is null.");
            }
            var vitalsModel = await _context.Vitals.FindAsync(id);
            if (vitalsModel != null && vitalsModel.FitnessUserId == currentUserId)
            {
                _context.Vitals.Remove(vitalsModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VitalsModelExists(int id)
        {
            return (_context.Vitals?.Any(e => e.VitalsId == id)).GetValueOrDefault();
        }
    }
}
