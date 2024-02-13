using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class LocationDto
    {
        [Required]
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [Required]
        [MaxLength(4)]
        public int[] Bbox { get; set; }

        [Required]
        public string MapBoxId { get; set; }


    }
}
