using System.Threading.Tasks;
using CinemaGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaGuide.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationContext context;
        public UsersController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpGet("users/{id}")]
        [Authorize]
        public async Task<ActionResult> Index([FromServices] Profile profile, int id)
        {
            var dbUser = await context.Users.FindAsync(id);
            if (dbUser == null)
            {
                return NotFound();
            }

            profile.User = dbUser;
            return View(profile);
        }
    }
}