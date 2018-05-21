using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CinemaGuide.Models;
using CinemaGuide.Models.Db;
using CinemaGuide.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaGuide.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationContext context;
        private readonly MailClient mailClient;

        public AuthController(ApplicationContext context, MailClient mailClient)
        {
            this.mailClient = mailClient;
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

        [HttpGet("/auth/forgot_password")]
        public IActionResult ForgotPassword([FromServices] Profile profile)
        {
            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(string login, string token)
        {
            var user = await TryGetUser(login);

            if (user == null)
            {
                return NotFound();
            }

            var expectedToken = PasswordEncryptor.GenerateHash($"{user.Id}{login}");

            if (expectedToken != token)
            {
                return Forbid();
            }

            user.IsConfirmed = true;
            context.Update(user);
            await context.SaveChangesAsync();

            await SetAuthCookie(user.Id, user.Login);
            await mailClient.SendGreeting(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(string login, string password, string confirmPassword)
        {
            var dbUser = await TryGetUser(login);
            var salt = PasswordEncryptor.GenerateSalt();
            dbUser.Salt = Convert.ToBase64String(salt);
            dbUser.Password = PasswordEncryptor.GenerateHash(password, salt);
            context.Update(dbUser);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword([FromServices] Profile profile, string login, string token)
        {
            var user = await TryGetUser(login);

            if (user == null)
            {
                return NotFound();
            }

            var expectedToken = PasswordEncryptor.GenerateHash($"{user.Id}{login}");

            if (expectedToken != token)
            {
                return Forbid();
            }

            profile.User = user;
            profile.User.Password = string.Empty;
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromServices] Profile profile, string login)
        {
            var user = await TryGetUser(login);

            if (user == null)
            {
                return NotFound();
            }
        
            await mailClient.SendResetPasswordLink(user);
            return View("PasswordResetInstruction", profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([FromServices] Profile profile, User user)
        {
            if (await context.Users.AnyAsync(x => x.Email == user.Email))
            {
                ModelState.AddModelError("User.Email", "к этой почте уже привязан существующий пользователь");
                return View(profile);
            }

            var dbUser = await TryGetUser(user.Login);

            profile.User = user;
            if (dbUser != null)
            {
                ModelState.AddModelError("UserCredentials.Login",
                    "Пользователь с таким логином уже существует");
                return View(profile);
            }

            var salt = PasswordEncryptor.GenerateSalt();

            profile.User.Role = "user";
            profile.User.Salt = Convert.ToBase64String(salt);
            profile.User.Password = PasswordEncryptor.GenerateHash(user.Password, salt);

            var entityEntry = context.Add(user);
            context.SaveChanges();

            await mailClient.SendConfirmLink(entityEntry.Entity);

            return View("Сonfirmation", profile); ;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromServices] Profile profile, Credentials credentials)
        {
            var user = await TryGetUser(credentials.Login);

            profile.Credentials = credentials;

            if (user == null)
            {
                ModelState.AddModelError("Credentials.Login", "Пользователя с таким логином не существует");

                return View(profile);
            }

            if (!user.IsConfirmed)
            {
                ModelState.AddModelError("Credentials.Login", "учетная запись не подтверждена");
                return View(profile);
            }

            if (!PasswordEncryptor.IsEqualPasswords(user.Password, user.Salt, credentials.Password))
            {
                ModelState.AddModelError("Credentials.Password", "Неверный пароль");

                return View(profile);
            }

            await SetAuthCookie(user.Id, credentials.Login);

            return RedirectToAction("Index", "Home");
        }

        private Task SetAuthCookie(int userId, string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, $"{userId}"),
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id));
        }

        private async Task<User> TryGetUser(string login)
        {
            return await context.Users.SingleOrDefaultAsync(user => user.Login == login);
        }
    }
}
