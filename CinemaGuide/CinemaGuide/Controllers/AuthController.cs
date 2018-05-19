using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CinemaGuide.Models;
using CinemaGuide.Models.Db;
using CinemaGuide.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaGuide.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationContext context;

        public AuthController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Login([FromServices] Profile profile)
        {
            return View(profile);
        }

        [HttpGet]
        public IActionResult SignUp([FromServices] Profile profile)
        {
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromServices] Profile profile, DbProfile userProfile)
        {
            var dbUser = await TryGetUser(userProfile.User.Login);

            if (dbUser != null)
            {
                ModelState.AddModelError("UserCredentials.Login",
                    "Пользователь с таким логином уже существует");
                profile.UserProfile = userProfile;

                return View(profile);
            }

            var salt = PasswordEncryptor.GenerateSalt();

            userProfile.Role = "user";
            userProfile.User.Salt = Convert.ToBase64String(salt);
            userProfile.User.Password = PasswordEncryptor.GenerateHash(
                userProfile.User.Password, salt); // TODO: fix

            context.Add(userProfile);
            context.SaveChanges();

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromServices] Profile profile, Credentials credentials)
        {
            var user = await TryGetUser(credentials.Login);

            profile.UserCredentials = credentials;

            if (user == null)
            {
                ModelState.AddModelError("UserCredentials.Login",
                    "Пользователя с таким логином не существует");

                return View(profile);
            }

            if (!PasswordEncryptor.IsEqualPasswords(user.Password, user.Salt, credentials.Password))
            {
                ModelState.AddModelError("UserCredentials.Password", "Неверный пароль");

                return View(profile);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimsIdentity.DefaultNameClaimType, credentials.Login)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id));

            return RedirectToAction("Index", "Home");
        }

        private async Task<DbUser> TryGetUser(string login)
        {
            return await context.Users.SingleOrDefaultAsync(user => user.Login == login);
        }
    }
}
