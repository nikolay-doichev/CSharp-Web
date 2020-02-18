using SIS.MvcFramework;
using System;
using System.Collections.Generic;

namespace SharedTrip.Models
{
    public class User : IdentityUser<string>
    {
        public User()
        {
            this.UserTrips = new HashSet<UserTrip>();
            this.Id = Guid.NewGuid().ToString();
        }
        public ICollection<UserTrip> UserTrips { get; set; }
    }
}
