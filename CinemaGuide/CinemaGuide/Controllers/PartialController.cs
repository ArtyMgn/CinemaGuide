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
        private readonly List<ICinemaApi> apiList;

        public PartialController(IEnumerable<ICinemaApi> apiCollection)
        {
            apiList = apiCollection.ToList();
        }

        [Route("search")]
        public async Task<IActionResult> Search(SearchConfig searchConfig)
        {
            var movies = new Dictionary<string, List<IMovieInfo>>();

            foreach (var api in apiList)
            {
                var foundMovies = await api.SearchAsync(searchConfig);

                if (!foundMovies.Any()) continue;

                movies.Add(api.Name, foundMovies);
            }

            return new JsonResult(movies);
        }
    }
}
