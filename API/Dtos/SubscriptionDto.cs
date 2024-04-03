using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class SubscriptionDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public bool CancelAtPeriodEnd { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string PlanType { get; set; }

        [Required]
        public decimal? Price { get; set; }
        public string SubscriptionItemId { get; set; }
    }
}
