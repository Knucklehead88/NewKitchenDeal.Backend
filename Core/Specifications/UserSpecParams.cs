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
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool ByDaily { get; set; }
        public bool ByHourly { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public List<string> MapBoxIds { get; set; }
        public List<int?> LanguageIds { get; set; }
        public List<int?> TradeIds { get; set; }

    }
}
