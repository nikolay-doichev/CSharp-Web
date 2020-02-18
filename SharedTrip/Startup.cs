namespace SharedTrip
{
    using System.Collections.Generic;
    using SharedTrip.Services;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class Startup : IMvcApplication
    {
        public void Configure(IList<Route> routeTable)
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();
        }

        public void ConfigureServices(IserviceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
            serviceCollection.Add<ITripService, TripService>();
        }
    }
}
