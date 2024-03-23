using Core.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Stripe
{
    public interface IPaymentMethodsService
    {
        Task<SetupIntent> CreateSetupIntent(string customerId, string paymentMethodId, string returnUrl = "");
        Task<PaymentMethod> AttachPaymentMethodToCustomerAsync(string customerId, string paymentMethodId);
        Task<PaymentMethod> DetachPaymentMethodFromCustomerAsync(string id);
        Task<PaymentMethod> UpdatePaymentMethodAsync(string id, Dictionary<string, string> metadata);
        Task<PaymentMethod> GetCustomerPaymentMethodAsync(string customerId, string paymentMethodId);
        Task<StripeList<PaymentMethod>> GetCustomerPaymentMethodsAsync(string customerId, int take);
    }
}
