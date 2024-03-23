using Stripe;

namespace Core.Interfaces.Stripe
{
    public interface IProductsService
    {
        Task<StripeList<Product>> GetProductsAsync(int take);
        Task<Product> GetProductAsync(string id);
        Task<Product> CreateProductAsync(string name);
        Task<Product> DeleteProductAsync(string id);
    }
}
