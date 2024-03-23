using Stripe;

namespace Core.Interfaces.Stripe
{
    public interface IPricesService
    {
        Task<StripeList<Price>> GetPricesAsync(int take);
        Task<Price> GetPriceAsync(string id);
        Task<Price> CreatePriceAsync(string productName, string currency, int unitAmount, string interval);
        Task<Price> UpdatePriceAsync(string id, Dictionary<string, string> metadata);


    }
}
