namespace API.Dtos
{
    public class PaymentMethodDto
    {
        public string Id { get; set; }
        public string Last4 { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }
}
