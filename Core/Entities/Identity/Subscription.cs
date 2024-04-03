namespace Core.Entities.Identity
{
    public class Subscription
    {
        public string Id { get; set; }
        public string PlanType { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CancelAtPeriodEnd { get; set; }
        public string Status { get; set; }
        public string SubscriptionItemId { get; set; }

    }
}