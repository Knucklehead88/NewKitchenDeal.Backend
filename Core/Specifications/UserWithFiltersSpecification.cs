using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class UserWithFiltersSpecification: BaseSpecification<AppUser>
    {
        public UserWithFiltersSpecification(UserSpecParams userParams)
        {
            ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1),
                userParams.PageSize);

            if (!string.IsNullOrEmpty(userParams.Sort))
            {
                switch (userParams.Sort)
                {
                    case "dailyAsc":
                        AddOrderBy(u => u.BusinessInfo.DailyRate);
                        break;
                    case "dailyDesc":
                        AddOrderByDescending(p => p.BusinessInfo.DailyRate);
                        break;
                    case "hourlyAsc":
                        AddOrderBy(u => u.BusinessInfo.HourlyRate);
                        break;
                    case "hourlyDesc":
                        AddOrderByDescending(p => p.BusinessInfo.HourlyRate);
                        break;
                    case "closes":
                    default:
                        AddOrderBy(n => n.UserName);
                        break;
                }
            }
        }
    }
}
