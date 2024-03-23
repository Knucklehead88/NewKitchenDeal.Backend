using Core.Entities;
using Core.Entities.Identity;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subscription = Stripe.Subscription;

namespace Core.Interfaces.Stripe
{
    public interface ISubscriptionsService
    {
        Task<Subscription> CreateSubscriptionAsync(string customerId, string priceId);
        Task<Subscription> CreateSubscriptionIntentAsync(string priceId, string customerId);
        Task<Subscription> UpdateSubscriptionAsync(string id, Dictionary<string, string> metadata);
        Task<Subscription> GetSubscriptionAsync(string id);
        Task<AppUser> CreateUserSubscriptionAsync(string email, string priceId, Subscription subscription);
        Task<StripeList<Subscription>> GetSubscriptionsAsync(int take);
        Task<Subscription> CancelSubscriptionsAsync(string id);
        Task<Subscription> ChangeSubscriptionAsync(string paymentMethodId, string subscriptionId, string priceId);
        Task<PaymentIntent> CompleteSubscriptionAsync(string paymentIntentId);
    }
}
