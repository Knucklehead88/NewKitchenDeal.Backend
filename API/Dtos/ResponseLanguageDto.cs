using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ResponseLanguageDto: LanguageDto
    {
        [Required]
        public string Name { get; set; }

    }
}
