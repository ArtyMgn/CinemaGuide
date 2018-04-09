using System.Threading.Tasks;
using CinemaGuide.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaGuide.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesAggregator moviesAggregator;

        public MoviesController(MoviesAggregator moviesAggregator)
        {
            this.moviesAggregator = moviesAggregator;
        }
        
        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var result = await moviesAggregator.SearchAsync(query);
            return new JsonResult(result);
        }
    }
}