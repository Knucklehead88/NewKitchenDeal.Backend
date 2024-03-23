using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class BusinessInfoDto
    {
        public string BusinessName { get; set; }
        public List<TradeDto> Trades { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string VideoPresentation { get; set; }
        public string Projects { get; set; }
        public List<LocationDto> Locations { get; set; }
        public List<LanguageDto> SpokenLanguages { get; set; }
        public long StartDateOfWork { get; set; }

    }
}
