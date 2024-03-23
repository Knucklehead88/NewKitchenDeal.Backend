using Core.Entities;
using Core.Interfaces.Stripe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Stripe
{
    [Authorize]
    public class CustomersService : ICustomersService
    {
        public CustomersService(MyAwsCredentials credentials)
        {
            // StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            StripeConfiguration.ApiKey = credentials.SecretKey;
        }

        public async Task<StripeList<Customer>> GetCustomersAsync(int take)
        {
            var options = new CustomerListOptions { Limit = take > 100 ? 100 : take };
            var service = new CustomerService();
            return await service.ListAsync(options);
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            var service = new CustomerService();
            var stripeCustomers = await service.ListAsync(new CustomerListOptions()
            {
                Email = email
            });

            if (!stripeCustomers.Any())     
                return null;   

            return stripeCustomers.FirstOrDefault();
        }


        public async Task<Customer> CreateCustomerAsync(string name, string email)
        {
            var service = new CustomerService();
            //var searchOptions = new CustomerSearchOptions
            //{
            //    Query = $"EMAIL:\"{email}\""
            //};
            //var result = await service.SearchAsync(searchOptions);
            //if (result.TotalCount > 0)
            //    return null;

            var options = new CustomerCreateOptions
            {
                Name = name,
                Email = email,
            };
            return await service.CreateAsync(options);
        }

        public async Task<Customer> UpdateCustomerAsync(string id, Dictionary<string, string> metadata)
        {
            var options = new CustomerUpdateOptions
            {
                Metadata = metadata,
            };
            var service = new CustomerService();
            return await service.UpdateAsync(id, options);
        }

        public async Task<Customer> DeleteCustomerAsync(string id)
        {
            var service = new CustomerService();
            return await service.DeleteAsync(id);
        }
    }
}
