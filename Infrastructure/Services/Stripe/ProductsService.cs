using Core.Entities;
using Core.Interfaces.Stripe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Stripe.Product;

namespace Infrastructure.Services.Stripe
{
    [Authorize]
    public class ProductsService : IProductsService
    {
        // private readonly IConfiguration _config;

        public ProductsService(MyAwsCredentials credentials)
        {
            // StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            StripeConfiguration.ApiKey = credentials.SecretKey;
        }

        public async Task<StripeList<Product>> GetProductsAsync(int take)
        {
            var options = new ProductListOptions { Limit = take > 100 ? 100 : take };
            var service = new ProductService();
            return await service.ListAsync(options);

        }

        public async Task<Product> GetProductAsync(string id)
        {
            var service = new ProductService();
            return await service.GetAsync(id);
        }

        public async Task<Product> CreateProductAsync(string name)
        {
            var options = new ProductCreateOptions { Name = name };
            var service = new ProductService();
            return await service.CreateAsync(options);
        }

        public async Task<Product> DeleteProductAsync(string id)
        {
            var service = new ProductService();
            return await service.DeleteAsync(id);
        }
    }
}
