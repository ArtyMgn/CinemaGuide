using CinemaGuide.Api;
using CinemaGuide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CinemaGuide.Controllers
{
    [Route("/search")]
    public class SearchController : Controller
    {
        private readonly List<ICinemaApi> apiList;

        public SearchController(IEnumerable<ICinemaApi> apiCollection)
        {
            apiList = apiCollection.ToList();
        }
        
        public async Task<IActionResult> Index(Profile profile)
        {
            var movies = new Dictionary<string, List<IMovieInfo>>();

            foreach (var api in apiList)
            {
                var foundMovies = await api.SearchAsync(profile.SearchConfig);

                if (!foundMovies.Any()) continue;

                movies.Add(api.Name, foundMovies);
            }

            return new JsonResult(movies);
        }
    }
}
