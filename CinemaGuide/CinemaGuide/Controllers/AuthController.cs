using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CinemaGuide.Models;
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
        public IActionResult Registration([FromServices] Profile profile)
        {
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromServices] Profile profile, DbProfile userProfile)
        {
            var matchedUser = await context.Users.SingleOrDefaultAsync(user => user.Login == userProfile.User.Login);
            if (matchedUser != null)
            {
                ModelState.AddModelError("UserCredentials.Login", "пользователь с таким логином уже существует");
                profile.UserProfile = userProfile;
                return View(profile);
            }

            var salt = PasswordEncryptor.GenerateSalt();
            userProfile.Role = "user";
            userProfile.User.Salt = Convert.ToBase64String(salt);
            userProfile.User.HashedPassword = PasswordEncryptor.GenerateHash(userProfile.User.HashedPassword, salt);
            context.Add(userProfile);
            context.SaveChanges();

            return RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromServices] Profile profile, LoginModel userCredentials)
        {
            var matchedUser = await context.Users.SingleOrDefaultAsync(user => user.Login == userCredentials.Login);
            profile.UserCredentials = userCredentials;
      
            if (matchedUser == null)
            {
                ModelState.AddModelError("UserCredentials.Login", "пользователя с таким логином не существует");
                return View(profile);
            }

            if (!PasswordEncryptor.IsEqualPasswords(matchedUser.HashedPassword, matchedUser.Salt, userCredentials.Password))
            {
                ModelState.AddModelError("UserCredentials.Password", "неверный пароль");
                return View(profile);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, matchedUser.Id),
                new Claim(ClaimsIdentity.DefaultNameClaimType, userCredentials.Login)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));


            return RedirectToAction("Index", "Home");
        }
    }
}