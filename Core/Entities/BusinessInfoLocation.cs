using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BusinessInfoLocation: BaseEntity
    {
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int BusinessInfoId { get; set; }
        public BusinessInfo BusinessInfo { get; set; }
    }
}
