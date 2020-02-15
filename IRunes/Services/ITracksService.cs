using IRunes.ViewModels.Tracks;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services
{
    public interface ITracksService
    {
        void Create(string albumId, string name, string link, decimal price);

        DetailsViewModel GetDetails(string trackId);
    }
}
