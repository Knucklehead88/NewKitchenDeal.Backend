using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ResponseTradeDto: TradeDto
    {
        [Required]
        public string Name { get; set; }
        public List<string> Categories { get; set; }

    }
}
