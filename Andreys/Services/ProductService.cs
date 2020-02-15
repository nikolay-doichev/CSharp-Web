using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andreys.Data;
using Andreys.Models;
using Andreys.ViewModels.Products;

namespace Andreys.Services
{
    public class ProductService : IProductService
    {
        private readonly AndreysDbContext dbContex;

        public ProductService(AndreysDbContext dbContex)
        {
            this.dbContex = dbContex;
        }
        public int Add(ProductAddInputModel productAddInputModel)
        {
            var genderAsEnum = Enum.Parse<Gender>(productAddInputModel.Gender);
            var categoryAsEnum = Enum.Parse<Category>(productAddInputModel.Category);
            
            var product = new Product()
            {
                Name = productAddInputModel.Name,
                Description = productAddInputModel.Description,
                ImageUrl = productAddInputModel.ImageUrl,
                Price = productAddInputModel.Price,
                Gender = genderAsEnum,
                Category = categoryAsEnum,
            };

            this.dbContex.Products.Add(product);
            this.dbContex.SaveChanges();

            return product.Id;
        }

        public IEnumerable<Product> GetAll()
        {
            var products = this.dbContex.Products
                .Select(x => new Product 
                {
                   Id = x.Id,
                   Name = x.Name,
                   ImageUrl = x.ImageUrl,
                   Price = x.Price,
                })
                .ToArray();

            return products;
        }

        public Product GetById(int id)
            => dbContex.Products
                .Where(p => p.Id == id).FirstOrDefault();

        public void DeleteById(int id)
        {
            var product = this.GetById(id);
            this.dbContex.Products.Remove(product);

            this.dbContex.SaveChanges();
        }
    }
}
