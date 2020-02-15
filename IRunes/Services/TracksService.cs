using IRunes.Models;
using IRunes.ViewModels.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.Services
{
    public class TracksService : ITracksService
    {
        private readonly ApplicationDbContext db;

        public TracksService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void Create(string albumId, string name, string link, decimal price)
        {
            var track = new Track
            {
                AlbumId = albumId,
                Name = name,
                Link = link,
                Price = price,
            };

            this.db.Tracks.Add(track);

            var allTracksPricesSum = this.db.Tracks
                .Where(x => x.AlbumId == albumId)
                .Sum(t => t.Price) + price;

            var album = this.db.Albums.Find(albumId);
            album.Price = allTracksPricesSum * 0.87M;

            this.db.SaveChanges();


        }

        public DetailsViewModel GetDetails(string trackId)
        {
            var track = this.db.Tracks
                .Where(t => t.Id == trackId)
                .Select(t => new DetailsViewModel
                {
                    Name = t.Name,
                    Link = t.Link,
                    AlbumId = t.AlbumId,
                    Price = t.Price,

                }).FirstOrDefault();

                return track;
        }
    }
}
