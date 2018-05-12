using CinemaGuide.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaGuide.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices] Profile profile)
        {
            return View(profile);
        }

        [Route("/error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
