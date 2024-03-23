using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces.Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subscription = Stripe.Subscription;

namespace Infrastructure.Services.Stripe
{
    public class SubscriptionsService: ISubscriptionsService
    {
        // private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPricesService _pricesService;

        public SubscriptionsService(MyAwsCredentials credentials, UserManager<AppUser> userManager, IPricesService pricesService)
        {
            _userManager = userManager;
            _pricesService = pricesService;
            // StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            StripeConfiguration.ApiKey = credentials.SecretKey;
        }

        public async Task<Subscription> CreateSubscriptionAsync(string customerId, string priceId)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items =
                [
                    new SubscriptionItemOptions { Price = priceId },
                ]
            };
            var service = new SubscriptionService();
            return await service.CreateAsync(options);
        }

        public async Task<PaymentIntent> CompleteSubscriptionAsync(string paymentIntentId)
        {
            var options = new PaymentIntentConfirmOptions
            {
                //PaymentMethod = "pm_card_visa",
                //ReturnUrl = "https://www.example.com",
            };
            var service = new PaymentIntentService();
            return await service.ConfirmAsync(paymentIntentId, options);
        }

        public async Task<AppUser> CreateUserSubscriptionAsync(string email, string priceId, Subscription subscription)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Subscription != null)
            {
                throw new ArgumentNullException($"{user.Email} already has a Subscription.");
            }
            var price = await _pricesService.GetPriceAsync(priceId);

            user.Subscription = new()
            {
                Id = subscription.Id,
                StartDate = subscription.Created,
                EndDate = subscription.CurrentPeriodEnd,
                Description = price.Product.Description,
                PlanType = price.Product.Name,
                Price = price.UnitAmountDecimal
            };

            await _userManager.UpdateAsync(user);
            return user;
        }

        public async Task<Subscription> CreateSubscriptionIntentAsync(string priceId, string customerId)
        {
            var paymentSettings = new SubscriptionPaymentSettingsOptions
            {
                SaveDefaultPaymentMethod = "on_subscription",
            };

            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items =
                [
                    new SubscriptionItemOptions
                    {
                        Price = priceId,
                    },
                ],
                PaymentSettings = paymentSettings,
                PaymentBehavior = "default_incomplete",
            };
            subscriptionOptions.AddExpand("latest_invoice.payment_intent");
            var subscriptionService = new SubscriptionService();

            return await subscriptionService.CreateAsync(subscriptionOptions);
        }

        public async Task<Subscription> UpdateSubscriptionAsync(string id, Dictionary<string, string> metadata)
        {

            var updateOptions = new SubscriptionUpdateOptions
            {
                Metadata = metadata
            };
            var service = new SubscriptionService();
            return await service.UpdateAsync(id, updateOptions);
        }

        public async Task<Subscription> GetSubscriptionAsync(string id)
        {
            var service = new SubscriptionService();
            return await service.GetAsync(id);
        }

        public async Task<StripeList<Subscription>> GetSubscriptionsAsync(int take)
        {
            var options = new SubscriptionListOptions { Limit = take > 100 ? 100 : take  };
            var service = new SubscriptionService();
            return await service.ListAsync(options);
        }

        public async Task<Subscription> CancelSubscriptionsAsync(string id)
        {
            var service = new SubscriptionService();

            return await service.CancelAsync(id);
        }

        public async Task<Subscription> ChangeSubscriptionAsync(string paymentMethodId, string subscriptionId, string priceId)
        {
            var service = new SubscriptionService();
            Subscription subscription = await service.GetAsync(subscriptionId);

            var items = new List<SubscriptionItemOptions>
            {
              new() {
                  Id = subscription.Items.Data[0].Id,
                  Price = priceId,
              },
            };

            var prorationDate = DateTime.Now;
            var options = new SubscriptionUpdateOptions
            {
                Items = items,
                ProrationDate = prorationDate,
            };

            if (!string.IsNullOrEmpty(paymentMethodId))
            {
                options.DefaultPaymentMethod = paymentMethodId;
            }

            return await service.UpdateAsync(subscriptionId, options);
        }
    }
}
