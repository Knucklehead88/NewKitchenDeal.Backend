using Core.Entities;
using Core.Interfaces.Stripe;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Services.Stripe
{
    public class InvoicesService : IInvoicesService
    {
        // private readonly IConfiguration _config;
        public InvoicesService(MyAwsCredentials credentials)
        {
            // StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            StripeConfiguration.ApiKey = credentials.SecretKey;
        }

        public async Task<Invoice> GetInvoiceAsync(string id)
        {
            var service = new InvoiceService();
            var options = new InvoiceGetOptions();
            options.AddExpand("subscription");
            return await service.GetAsync(id, options);
        }

        public async Task<StripeSearchResult<Invoice>> GetInvoicesForCustomerAsync(string customerId)
        {
            var searchOptions = new InvoiceSearchOptions { Query = $"customer:\"{customerId}\" and status:\"paid\"" };
            searchOptions.AddExpand("data.subscription");
            var service = new InvoiceService();
            return await service.SearchAsync(searchOptions);
        }
    }
}
