using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ResponseBusinessInfoDto
    {
        [Required]
        public string BusinessName { get; set; }
        public List<ResponseTradeDto> Trades { get; set; }
        public string HourlyRate { get; set; }
        public string DailyRate { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string VideoPresentation { get; set; }
        public string Projects { get; set; }
        public List<LocationDto> Locations { get; set; }
        public List<ResponseLanguageDto> SpokenLanguages { get; set; }
        public long StartDateOfWork { get; set; }

    }
}
