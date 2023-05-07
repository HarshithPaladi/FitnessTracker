﻿using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [HttpGet("Search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<WorkoutsModel>>> SearchWorkouts(string searchTerm)
        {
            var workouts = await _context.Workouts
                .Where(w => EF.Functions.Like(w.Name, $"%{searchTerm}%")
                    || EF.Functions.Like(w.Type, $"%{searchTerm}%")
                    || EF.Functions.Like(w.Description, $"%{searchTerm}%")
                    || EF.Functions.Like(w.CaloriesBurned.ToString(), $"%{searchTerm}%"))
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