using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.ViewModels.Trip
{
    public class AllTripsViewModel
    {
        public IEnumerable<TripsInfoViewModel> AllTrips { get; set; }
    }
}
