using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace FitnessTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutSearchController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public WorkoutSearchController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult Index(string searchTerm)
        {
            // Create an instance of the HttpClient class using the factory
            var client = _clientFactory.CreateClient();

            // Call the WorkoutsSearchAPI using the searchTerm
            var response = client.GetAsync($"https://localhost:5001/api/WorkoutsSearchAPI/{searchTerm}").Result;

            // Deserialize the response body
            var workouts = response.Content.ReadAsAsync<IEnumerable<WorkoutsModel>>().Result;

            // Pass the list of workouts to the view
            return View(workouts);
        }
    }

}
