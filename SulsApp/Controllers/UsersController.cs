using System;
using System.Net.Mail;

using SIS.HTTP;
using SulsApp.Services;
using SIS.MvcFramework;
using SIS.HTTP.Logging;
using SulsApp.ViewModels.Users;

namespace SulsApp.Controllers
{
    public class UsersController : Controller
    {
        private IUsersService usersService;
        private ILogger logger;

        public UsersController(IUsersService usersService, ILogger logger)
        {
            this.usersService = usersService;
            this.logger = logger;
        }
        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            var userId = this.usersService.GetUserId(username, password);
            if (userId == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SingIn(userId);
            this.logger.Log("User logged in: " + username);
            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {
            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Passwords should be the same!");
            }

            if (input.Username?.Length < 5 || input.Username?.Length > 20)
            {
                return this.Error("Username should be between 5 and 20 characters.");
            }

            if (!IsValid(input.Email))
            {
                return this.Error("Invalid email!");
            }          

            if (input.Password?.Length < 6 || input.Password?.Length > 20)
            {
                return this.Error("Password should be between 6 and 20 characters.");
            }

            if (this.usersService.IsUserNameUsed(input.Username))
            {
                return this.Error("Username already exist!");
            }

            if (this.usersService.IsEmailUsed(input.Email))
            {
                return this.Error("Email already exist!");
            }

            this.usersService.CreateUser(input.Username, input.Email, input.Password);
            this.logger.Log("New user: " + input.Username);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.SingOut();
            return this.Redirect("/");
        }

        private bool IsValid(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
