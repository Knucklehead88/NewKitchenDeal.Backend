using API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CreatePaymentMethod
    {
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public Card Card { get; set; }
    }
}
