using Core.Entities;
using Core.Interfaces.Stripe;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Stripe
{
    public class PaymentMethodsService : IPaymentMethodsService
    {
        // private readonly IConfiguration _config;
        public PaymentMethodsService(MyAwsCredentials credentials)
        {
            // StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            StripeConfiguration.ApiKey = credentials.SecretKey;
        }

        public async Task<SetupIntent> CreateSetupIntent(string customerId, string paymentMethodId, string returnUrl = "")
        {
            var options = new SetupIntentCreateOptions
            {
                PaymentMethodTypes = ["card"],
                Customer = customerId,
                PaymentMethod = paymentMethodId,
                Confirm = true,
                ReturnUrl = returnUrl
            };
            var service = new SetupIntentService();
            return await service.CreateAsync(options);
        }

        public async Task<PaymentMethod> AttachPaymentMethodToCustomerAsync(string customerId, string paymentMethodId)
        {
            var service = new PaymentMethodService();
            var paymentMethod = await service.GetAsync(paymentMethodId);

            var optionsPaymentMethodAttach = new PaymentMethodAttachOptions { Customer = customerId };
            service.Attach(paymentMethodId, optionsPaymentMethodAttach);

            return paymentMethod;
        }

        public async Task<PaymentMethod> DetachPaymentMethodFromCustomerAsync(string id)
        {
            var service = new PaymentMethodService();
            return await service.DetachAsync(id);
        }

        public async Task<PaymentMethod> UpdatePaymentMethodAsync(string id, Dictionary<string, string> metadata)
        {
            var options = new PaymentMethodUpdateOptions
            {
                Metadata = metadata,
            };
            var service = new PaymentMethodService();
            return await service.UpdateAsync(id, options);
        }

        public async Task<PaymentMethod> GetCustomerPaymentMethodAsync(string customerId, string paymentMethodId)
        {
            var service = new CustomerService();
            return await service.RetrievePaymentMethodAsync(customerId, paymentMethodId);
        }

        public async Task<StripeList<PaymentMethod>> GetCustomerPaymentMethodsAsync(string customerId, int take)
        {
            var options = new CustomerListPaymentMethodsOptions { Limit = take > 100 ? 100 : take  };
            var service = new CustomerService();
            return await service.ListPaymentMethodsAsync(customerId, options);
        }
    }
}
