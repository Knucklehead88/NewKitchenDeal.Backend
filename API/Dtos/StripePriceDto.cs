using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class StripePriceDto
    {
        [Required]
        public string Id { get; set; }

        [StringLength(3)]
        public string Currency { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public string UnitAmount { get; set; }
        public string Interval { get; set; }
        public string Type { get; set; }
    }
}
