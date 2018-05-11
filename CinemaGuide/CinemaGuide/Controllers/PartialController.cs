using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaGuide.Api;
using Microsoft.AspNetCore.Mvc;

namespace CinemaGuide.Controllers
{
    [Route("partial")]
    public class PartialController : Controller
    {
        private readonly Dictionary<string, ICinemaApi> sources;

        public PartialController(IEnumerable<ICinemaApi> api)
        {
            sources = api.ToDictionary(a => a.GetType().Name);
        }

        [Route("search")]
        public async Task<IActionResult> Search(SearchConfig searchConfig, string sourceName)
        {
            if (!sources.ContainsKey(sourceName))
            {
                Console.WriteLine($"Server haven't source with name '{sourceName}'"); // Need to log this
                return View(new List<IMovieInfo>());
            }

            var source = sources[sourceName];
            var movies = await source.SearchAsync(searchConfig);

            return View(movies);
        }
    }
}
