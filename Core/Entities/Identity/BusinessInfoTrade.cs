using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class BusinessInfoTrade: BaseEntity
    {
        public int TradeId { get; set; }
        public Trade Trade { get; set; }
        public int BusinessInfoId { get; set; }
        public BusinessInfo BusinessInfo { get; set; }
    }
}
