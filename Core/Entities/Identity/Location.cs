using System.Reflection.Metadata;

namespace Core.Entities.Identity
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int? PersonalInfoId { get; set; } // Optional foreign key property
        public PersonalInfo? PersonalInfo { get; set; }
        public int? BusinesslInfoId { get; set; } // Optional foreign key property
        public BusinessInfo? BusinessInfo { get; set; }

    }
}