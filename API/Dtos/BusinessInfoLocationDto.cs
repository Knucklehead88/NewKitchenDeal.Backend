using Core.Entities.Identity;

namespace API.Dtos
{
    public class BusinessInfoLocationDto
    {
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int BusinessInfoId { get; set; }
        public BusinessInfo BusinessInfo { get; set; }
    }
}
