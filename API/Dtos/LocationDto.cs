using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class LocationDto
    {
        [Required]
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
