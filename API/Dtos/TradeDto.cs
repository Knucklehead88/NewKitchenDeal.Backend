using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class TradeDto
    {
        [Required]
        public int Id { get; set; }
    }
}
