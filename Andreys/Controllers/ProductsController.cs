using Andreys.Services;
using Andreys.ViewModels.Products;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Andreys.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productsService;

        public ProductsController(IProductService productsService)
        {
            this.productsService = productsService;
        }
        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(ProductAddInputModel inputModel)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(inputModel.Name) || inputModel.Name.Length < 4 || inputModel.Name.Length > 20)
            {
                return this.View();
            }

            if (string.IsNullOrWhiteSpace(inputModel.Name) || inputModel.Description.Length > 10)
            {
                return this.View();
            }
            var productId = this.productsService.Add(inputModel);

            return this.Redirect("/Products/Details?id=" + productId);
        }

        public HttpResponse Details(int id)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var product = this.productsService.GetById(id);

            return this.View(product);
        }

        public HttpResponse Delete(int id)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            this.productsService.DeleteById(id);

            return this.Redirect("/");
        }
    }
}
