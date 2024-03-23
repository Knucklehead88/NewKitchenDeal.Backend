using Core.Entities;
using Core.Interfaces.Stripe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Services.Stripe
{
    public class PricesService : IPricesService
    {
        // private readonly IConfiguration _config;
        public PricesService(MyAwsCredentials credentials)
        {
            // StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            StripeConfiguration.ApiKey = credentials.SecretKey;
        }

        public async Task<Price> CreatePriceAsync(string productName, string currency, int unitAmount, string interval)
        {

            var options = new PriceCreateOptions
            {
                Currency = currency,
                UnitAmount = unitAmount,
                Recurring = new PriceRecurringOptions { Interval = interval },
                ProductData = new PriceProductDataOptions { Name = productName },
            };
            var service = new PriceService();
            return await service.CreateAsync(options);
        }

        public async Task<Price> UpdatePriceAsync(string id, Dictionary<string, string> metadata)
        {

            var updateOptions = new PriceUpdateOptions
            {
                Metadata = metadata
            };
            var service = new PriceService();
            return await service.UpdateAsync(id, updateOptions);
        }

        public async Task<Price> GetPriceAsync(string id)
        {
            var service = new PriceService();
            var options = new PriceGetOptions();
            options.AddExpand("product");
            return await service.GetAsync(id, options);
        }

        public async Task<StripeList<Price>> GetPricesAsync(int take)
        {
            var options = new PriceListOptions { Limit = take > 0 || take < 100 ? take : 100 };
            var service = new PriceService();
            return await service.ListAsync(options);
        }
    }
}
