using System;
using System.Threading.Tasks;
using CinemaGuide.Models.Db;
using Microsoft.AspNetCore.Http;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CinemaGuide.Utils
{
    public class MailClient
    {
        private readonly SendGridClient sendGridClient;
        private readonly EmailAddress senderEmailAddress;
        private readonly string baseUrl;

        public MailClient(string apiKey, string baseUrl, string senderEmail="sinemaguide@support.com")
        {
            this.baseUrl = baseUrl;
            senderEmailAddress = new EmailAddress(senderEmail, "CinemaGuide");
            sendGridClient = new SendGridClient(apiKey);
        }

        public async Task<Response> SendConfirmLink(User user)
        {
            var confirmUrl = GenerateRedirectLink(user.Id, user.Login, "confirm", "auth");
            var subject = "Подтвердите регистрацию в CinemaGuide";
            var to = new EmailAddress(user.Email);
            var htmlContent = $"<a href=\"{confirmUrl}\">подтвердить регистрацию</a>";
            var message = MailHelper.CreateSingleEmail(senderEmailAddress, to, subject, "", htmlContent);

            return await sendGridClient.SendEmailAsync(message);
        }

        public async Task<Response> SendResetPasswordLink(User user)
        {
            var confirmUrl = GenerateRedirectLink(user.Id, user.Login, "ResetPassword", "Auth");
            var subject = "Сброс пароля";
            var to = new EmailAddress(user.Email);
            var htmlContent = $"Пожалуйста, пройдите по <a href=\"{confirmUrl}\">ссылке</a> и установите новый пароль";
            var message = MailHelper.CreateSingleEmail(senderEmailAddress, to, subject, "", htmlContent);

            return await sendGridClient.SendEmailAsync(message);
        }

        public async Task<Response> SendGreeting(User user)
        {
            var subject = "Добро пожаловать в CinemaGuide";
            var to = new EmailAddress(user.Email);
            var greetingMessage = $"Добро пожаловать в CinemaGuide, {user.Login}! Мы рады, что вы теперь с нами :)";
            var message = MailHelper.CreateSingleEmail(senderEmailAddress, to, subject, greetingMessage, "");
            return await sendGridClient.SendEmailAsync(message);
        }

        private Uri GenerateRedirectLink(int userId, string login, string action, string controller)
        {
            var token = PasswordEncryptor.GenerateHash($"{userId}{login}");
            var queryString = new QueryString()
                .Add("login", login)
                .Add("token", token);
            return new Uri($"{baseUrl}/{controller}/{action}{queryString.ToUriComponent()}");
        }
    }
}
