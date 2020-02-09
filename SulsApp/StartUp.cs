using System;
using System.Collections.Generic;

using SIS.HTTP;
using SIS.HTTP.Logging;
using SIS.MvcFramework;
using SulsApp.Services;

namespace SulsApp
{
    public class StartUp : IMvcApplication
    {

        public void ConfigureServices(IserviceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
        }

        public void Configure(IList<Route> routeTable)
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();
        }
    }
}
