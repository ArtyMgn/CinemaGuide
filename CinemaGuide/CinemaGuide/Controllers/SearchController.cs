using CinemaGuide.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaGuide.Controllers
{
    [Route("/search")]
    public class SearchController : Controller
    {
        public IActionResult Index(Profile profile)
        {
            return View(profile);
        }
    }
}
