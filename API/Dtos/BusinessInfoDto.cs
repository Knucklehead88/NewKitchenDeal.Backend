using Core.Entities.Identity;

namespace API.Dtos
{
    public class BusinessInfoDto
    {
        public string BusinessName { get; set; }
        public IReadOnlyList<TradeDto> Trades { get; set; }
        public string HourlyRate { get; set; }
        public string DailyRate { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string VideoPresentation { get; set; }
        public string Projects { get; set; }
        public IReadOnlyList<LocationDto> Locations { get; set; }
        public string[] SpokenLanguages { get; set; }
        public int YearsOfExperience { get; set; }

    }
}
