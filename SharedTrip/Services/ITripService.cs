using SharedTrip.Models;
using SharedTrip.ViewModels.Trip;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Services
{
    public interface ITripService
    {
        void Add(TripAddInputModel tripAddInputModel);

        IEnumerable<Trip> GetAll();

        Trip GetById(int id);

        TripsInfoViewModel GetDetails(string id);
        
    }
}
