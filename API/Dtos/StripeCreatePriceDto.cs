using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class StripeCreatePriceDto
    {

        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public int UnitAmount { get; set; }
        public string Interval { get; set; }
    }
}
