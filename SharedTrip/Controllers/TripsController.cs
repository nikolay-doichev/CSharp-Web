using SharedTrip.Services;
using SharedTrip.ViewModels.Trip;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripService tripService;

        public TripsController(ITripService tripService)
        {
            this.tripService = tripService;
        }
        public HttpResponse All()
        {
            //Only Logged in Users can see all the trips!
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var allProducts = tripService.GetAll();

            return this.View(allProducts);
        }

        public HttpResponse Add()
        {
            //Only Logged in users can see add form menu!
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(TripAddInputModel input)
        {
            //Security that track if user is not logged in he cannot add trip
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            //Validation for seats
            if (input.Seats < 2 || input.Seats > 6 )
            {
                return this.View();
                //A possible way to say what is wrong with adding new trip 
                //return this.Error("The seats range must be between 2 and 6!");
            }

            if (input.Description.Length > 80)
            {
                return this.View();
                //A nice way to say to the user for wrong input
                //return this.Error("The Description must be below 80 characters please!");
            }

            this.tripService.Add(input);

            return this.View("/Trips/All");
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.tripService.GetDetails(tripId);
            return this.View(viewModel);
        }

        
    }
}
