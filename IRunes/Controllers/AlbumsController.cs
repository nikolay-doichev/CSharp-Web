using IRunes.Services;
using IRunes.ViewModels.Albums;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumsService albumsService;

        public AlbumsController(IAlbumsService albumsService)
        {
            this.albumsService = albumsService;
        }
        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var viewModel = new AllAlbumsViewModels
            {
                Albums = this.albumsService.GetAll(x => new AlbumInfoViewModel 
                {
                    Id = x.Id,
                    Name = x.Name,
                }),
            };

            return this.View(viewModel);
        }

        public HttpResponse Create()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            return this.View();
        }

        public HttpResponse Details(string id)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var viewModel = this.albumsService.GetDetails(id);
            return this.View(viewModel);
        }

        [HttpPost]
        public  HttpResponse Create(CreateInputModel input)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            if (input.Name.Length < 4 || input.Name.Length > 20)
            {
                return this.Error("Name should be with lenght between 4 and 20!");
            }

            if (string.IsNullOrWhiteSpace(input.Cover))
            {
                return this.Error("Cover is required!");
            }

            this.albumsService.Create(input.Name, input.Cover);

            return this.Redirect("/Albums/All");
        }
    }
}
