using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SharedTrip.Models;
using SharedTrip.ViewModels.Trip;
using SharedTrip.ViewModels.UserTrip;

namespace SharedTrip.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext db;
        private readonly IUsersService usersService;

        public TripService(ApplicationDbContext db, IUsersService usersService)
        {
            this.db = db;
            this.usersService = usersService;
        }
        public void Add(TripAddInputModel tripAddInputModel)
        {
            var trip = new Trip
            {
                StartPoint = tripAddInputModel.StartPoint,
                EndPoint = tripAddInputModel.EndPoint,
                DepartureTime = DateTime.ParseExact(tripAddInputModel.DepartureTime.ToString(), "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                ImagePath = tripAddInputModel.ImagePath,
                Seats = tripAddInputModel.Seats,
                Description = tripAddInputModel.Description,
            };

            this.db.Trips.Add(trip);
            this.db.SaveChanges();
        }
        

        public IEnumerable<Trip> GetAll()
        {
            var products = this.db.Trips
                .Select(x => new Trip
                {
                    Id = x.Id,
                    StartPoint = x.StartPoint,
                    EndPoint = x.EndPoint,
                    DepartureTime = x.DepartureTime,
                    Seats = x.Seats,
                    Description = x.Description,
                })
                .ToArray();

            return products;
        }

        public Trip GetById(int id)
            => this.db.Trips.Where(t => t.Id == id.ToString()).FirstOrDefault();

        public void GetDetails(int id)
        {
            var trip = this.GetById(id);
            this.db.Trips.Remove(trip);

            this.db.SaveChanges();
        }

        public TripsInfoViewModel GetDetails(string id)
        {
            var trip = this.db.Trips
                .Where(x => x.Id == id)
                .Select(x => new TripsInfoViewModel
                {
                    Id = x.Id,
                    StartPoint = x.StartPoint,
                    EndPoint = x.EndPoint,
                    DepartureTime = x.DepartureTime.ToString(),
                    ImagePath = x.ImagePath,
                    Seats = x.Seats.ToString(),
                    Description = x.Description,
                })
                .FirstOrDefault();
                    
            return trip;
        }
    }
}
