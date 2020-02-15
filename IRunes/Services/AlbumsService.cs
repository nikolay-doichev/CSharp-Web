using IRunes.Models;
using IRunes.ViewModels.Albums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.Services
{
    public class AlbumsService : IAlbumsService
    {
        private readonly ApplicationDbContext db;

        public AlbumsService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void Create(string name, string cover)
        {
            var album = new Album
            {
                Name = name,
                Cover = cover,
                Price = 0,
            };

            this.db.Albums.Add(album);
            this.db.SaveChanges();
        }

        public IEnumerable<T> GetAll<T>(Func<Album, T> selectedFunct)
        {
            var allAlbuns = this.db.Albums.Select(selectedFunct).ToList();

            return allAlbuns;
        }

        public AlbumDetailsViewModel GetDetails(string id)
        {
            var album = this.db.Albums
                .Where(x => x.Id == id)
                .Select(a => new AlbumDetailsViewModel 
                {
                    Id = a.Id,
                    Name = a.Name,
                    Cover = a.Cover,
                    Price = a.Price,
                    Tracks = a.Tracks.Select(xt => new TrackInfoViewModel 
                    {
                        Id = xt.Id,
                        Name = xt.Name,
                    })
                })
                .FirstOrDefault();

            return album;
        }
    }
}
