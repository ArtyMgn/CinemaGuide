using CinemaGuide.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaGuide.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index(string query, [FromServices] Profile profile)
        {
            profile.SearchConfig.Query = query;

            return View(profile);
        }
    }
}
