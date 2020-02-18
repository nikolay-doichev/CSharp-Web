using SharedTrip.Services;
using SharedTrip.ViewModels.User;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        public HttpResponse Login()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginInputModel input)
        {
            var userId = this.usersService.GetUserId(input.Username, input.Password);
            if (userId != null)
            {
                this.SingIn(userId);
                return this.Redirect("/Trips/All");
            }

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Register()
        {
            //Check if User is LoggedIn. If he is redirect him to home page and restrict him to see register,
            //because only Guest can register new User
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.Email))
            {
                return this.Error("Email cannot be empty!");
            }

            if (input.Password.Length < 6 || input.Password.Length > 20)
            {
                return this.Error("Password must be at least 6 characters at most 20");
            }

            if (input.Username.Length < 5 || input.Username.Length > 20)
            {
                return this.Error("Username must be at least 5 characters at most 20");
            }

            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Passwords should match!");
            }

            if (this.usersService.EmailExists(input.Email))
            {
                return this.Error("Email already in use.");
            }

            if (this.usersService.UsernameExist(input.Username))
            {
                return this.Error("Username already in use.");
            }

            this.usersService.Register(input.Username, input.Email, input.Password);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            //Only Logged in users can see LogOut
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }
            this.SingOut();
            return this.Redirect("/");
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.AddUserToTrip(tripId);

            return this.Redirect("/");
        }
    }
}
