using CinemaGuide.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaGuide.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var defaultProfileInfo = new ProfileInfo();

            return View(defaultProfileInfo);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
