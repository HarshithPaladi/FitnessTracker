using System.Security.Claims;
using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VitalsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<FitnessUser> _userManager;

        public VitalsAPIController(ApplicationDbContext context, UserManager<FitnessUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: api/VitalsAPI/GetVitals?days=7&type=BP
        [HttpGet, Route("GetVitals")]
        [Authorize]
        public async Task<IActionResult> GetVitals(int days, string? type = "")
        {
            // Get the user's ID from the access token or session cookie
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Calculate the date range
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddDays(-days).Date;

            // Retrieve the vitals records for the specified type and date range
            IQueryable<VitalsModel> vitalsQuery = _context.Vitals
                .Where(v => v.FitnessUserId == userId && v.Date >= startDate && v.Date <= endDate);

            if (type == "BP")
            {
                // Return only records that have both systolic and diastolic BP values and only return those values
                vitalsQuery = vitalsQuery.Where(v => v.SystolicBP != null && v.DiastolicBP != null)
                    .Select(v => new VitalsModel
                    {
                        VitalsId = v.VitalsId,
                        Date = v.Date,
                        SystolicBP = v.SystolicBP,
                        DiastolicBP = v.DiastolicBP,
                        FitnessUserId = v.FitnessUserId
                    });
            }
            else if (type == "heartRate")
            {
                vitalsQuery = vitalsQuery.Where(v => v.HeartRate != null)
                    .Select(v => new VitalsModel
                    {
                        VitalsId = v.VitalsId,
                        Date = v.Date,
                        HeartRate = v.HeartRate,
                        FitnessUserId = v.FitnessUserId
                    });
            }
            else if (type == "oxygenSaturation")
            {
                vitalsQuery = vitalsQuery.Where(v => v.OxygenSaturation != null)
                    .Select(v => new VitalsModel
                    {
                        VitalsId = v.VitalsId,
                        Date = v.Date,
                        OxygenSaturation = v.OxygenSaturation,
                        FitnessUserId = v.FitnessUserId
                    });
            }
            else if (!string.IsNullOrEmpty(type))
            {
                vitalsQuery = vitalsQuery.Where(v => v.GetType().GetProperty(type) != null);
            }

            var vitalsList = vitalsQuery.ToList().OrderBy(v => v.Date);

            return Ok(vitalsList);
        }
        //[HttpGet("chart")]
        //public IActionResult GetVitalsChart(int days, string type)
        //{
        //    // Get the user's ID from the access token or session cookie
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    // Get data from API : api/VitalsAPI/GetVitals?days=7&type=BP
        //    var vitals = GetVitals(days, type);
        //    // use this data to create chart
            


        //    // // Create the chart
        //    // var chart = new Chart
        //    // {
        //    //     Type = "line",
        //    //     Data = new Data
        //    //     {
        //    //         Labels = groupedVitals.Select(g => g.Date.ToShortDateString()).ToArray(),
        //    //         Datasets = new Dataset[]
        //    //         {
        //    //     new Dataset
        //    //     {
        //    //         Label = type,
        //    //         Data = groupedVitals.Select(g => g.Value).ToArray(),
        //    //         BackgroundColor = "rgba(54, 162, 235, 0.2)",
        //    //         BorderColor = "rgba(54, 162, 235, 1)",
        //    //         BorderWidth = 1
        //    //     }
        //    //         }
        //    //     },
        //    //     Options = new Options
        //    //     {
        //    //         Scales = new Scales
        //    //         {
        //    //             YAxes = new[]
        //    //             {
        //    //         new YAxis
        //    //         {
        //    //             Ticks = new Ticks
        //    //             {
        //    //                 BeginAtZero = true
        //    //             }
        //    //         }
        //    //     }
        //    //         }
        //    //     }
        //    // };

        //    // Return the chart as a JSON response
        //    return Json(chart);
        //}

    }
}
