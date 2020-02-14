﻿using IRunes.Services;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes
{
    public class Startup : IMvcApplication
    {
        public void Configure(IList<Route> routeTable)
        {
            var db = new ApplicationDbContext();
            db.Database.Migrate();
        }

        public void ConfigureServices(IserviceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
            serviceCollection.Add<IAlbumsService, AlbumsService>();
            serviceCollection.Add<ITracksService, TracksService>();
        }
    }
}
