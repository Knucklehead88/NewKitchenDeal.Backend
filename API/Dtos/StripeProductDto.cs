namespace API.Dtos
{
    public class StripeProductDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public long? Price { get; set; }
    }
}
