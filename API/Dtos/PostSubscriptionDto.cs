using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PostSubscriptionDto: SubscriptionDto
    {
        [Required]
        public string PriceId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        public string Status { get; set; }
        public decimal? UnitAmount { get; set; }

        [Required]
        public string ProductId { get; set; }


    }
}
