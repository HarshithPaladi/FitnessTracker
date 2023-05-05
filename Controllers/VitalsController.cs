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
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(this.User);

            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var vitals = await _context.Vitals
                .Where(w => w.FitnessUserId == currentUserId)
                .ToListAsync();

            return View(vitals);
        }

        // GET: Vitals/Details/5
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
        public IActionResult Create()
        {
            var user = _userManager.GetUserAsync(User).Result;
            ViewBag.CurrentUserId = user.Id;

            return View();
        }

        // POST: Vitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Vitals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vitals == null)
            {
                return NotFound();
            }

            var vitalsModel = await _context.Vitals.FindAsync(id);
            if (vitalsModel == null)
            {
                return NotFound();
            }
            return View(vitalsModel);
        }

        // POST: Vitals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VitalsId,Date,HeartRate,SystolicBP,DiastolicBP,OxygenSaturation,FitnessUserId")] VitalsModel vitalsModel)
        {
            if (id != vitalsModel.VitalsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vitalsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VitalsModelExists(vitalsModel.VitalsId))
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
            return View(vitalsModel);
        }

        // GET: Vitals/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Vitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vitals == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vitals'  is null.");
            }
            var vitalsModel = await _context.Vitals.FindAsync(id);
            if (vitalsModel != null)
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
