namespace Core.Entities.Identity
{
    public class AppUserLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }
        public int PersonalInfoId { get; set; }
        public PersonalInfo PersonalInfo { get; set; }

    }
}