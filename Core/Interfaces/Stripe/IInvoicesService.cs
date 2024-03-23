using Stripe;

namespace Core.Interfaces.Stripe
{
    public interface IInvoicesService
    {
        Task<Invoice> GetInvoiceAsync(string id);
        Task<StripeSearchResult<Invoice>> GetInvoicesForCustomerAsync(string customerId);
    }
}