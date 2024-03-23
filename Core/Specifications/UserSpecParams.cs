using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class UserSpecParams: BaseSpecParams
    {
        public decimal DailyRate { get; set; }
        public decimal HourlyRate { get; set; }
        public string DailyRateRange { get; set; }
        public string HourlyRateRange { get; set; }
        public double? Closes { get; set; }
        public int? LanguageId { get; set; }
        public int? TradeId { get; set; }

    }
}
