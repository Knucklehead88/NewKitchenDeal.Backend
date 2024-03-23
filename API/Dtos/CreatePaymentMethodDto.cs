namespace API.Dtos
{
    public class CreatePaymentMethodDto
    {
        public string Type { get; set; }
        public CardDto Card { get; set; }
    }
}
