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

        public IActionResult Error()
        {
            return View();
        }
    }
}
