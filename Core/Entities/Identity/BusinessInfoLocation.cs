using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class BusinessInfoLanguage: BaseEntity
    {
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int BusinessInfoId { get; set; }
        public BusinessInfo BusinessInfo { get; set; }
    }
}
