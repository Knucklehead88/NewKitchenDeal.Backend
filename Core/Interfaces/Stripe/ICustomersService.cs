using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Stripe
{
    public interface ICustomersService
    {
        Task<StripeList<Customer>> GetCustomersAsync(int take);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<Customer> CreateCustomerAsync(string name, string email);
        Task<Customer> UpdateCustomerAsync(string id, Dictionary<string, string> metadata);
        Task<Customer> DeleteCustomerAsync(string id);
    }
}
