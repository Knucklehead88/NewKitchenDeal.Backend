using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using API.Dtos;
using Core.Entities.Identity;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Geometries;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserByClaimsPrincipleWithAddress(this UserManager<AppUser> userManager, 
            ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users.Include(x => x.Address)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindUserByClaimsPrincipleWithPersonalInfo(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users
                .Include(x => x.Subscription)
                .Include(x => x.PersonalInfo)
                    .ThenInclude(x => x.Locations)
                        .ThenInclude(x => x.Location)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindUserByClaimsPrincipleWithBusinessInfo(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users
                .Include(x => x.Subscription)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Locations)
                        .ThenInclude(x => x.Location)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Trades)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.SpokenLanguages)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> userManager, 
            ClaimsPrincipal user)
        {
            return await userManager.Users
                .Include(x => x.Subscription)
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        }

        public static async Task<AppUser> FindByEmailWithSubscription(this UserManager<AppUser> userManager, 
            string Email)
        {
            return await userManager.Users
                .Include(x => x.Subscription)
                .SingleOrDefaultAsync(x => x.Email == Email);
        }

        public static async Task<AppUser> FindByEmailWithSubscriptionAndBusinessInfo(this UserManager<AppUser> userManager,
            string email)
        {
            return await userManager.Users
                .Include(x => x.Subscription)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Locations)
                        .ThenInclude(x => x.Location)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Trades)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.SpokenLanguages)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindByLoginProviderWithSubscription(this UserManager<AppUser> userManager,
        string loginProvider, string providerKey)
        {
            var user = await userManager.FindByLoginAsync(loginProvider, providerKey);
            return await userManager?.FindByEmailWithSubscription(user.Email);
        }


        public static async Task<IReadOnlyList<AppUser>> FindByUserParams(this UserManager<AppUser> userManager,
            UserSpecParams userParams)
        {
            var spec = new UserWithFiltersSpecification(userParams);

            var query = userManager.Users.AsQueryable();
            var minPrice = userParams.MinPrice > 0 ? userParams.MinPrice : default;
            var maxPrice = userParams.MaxPrice > 0 ? userParams.MaxPrice : decimal.MaxValue;

            if (userParams.ByDaily)
            {
                query = query.Where(x => x.BusinessInfo.DailyRate >= minPrice && x.BusinessInfo.DailyRate <= maxPrice);
            }

            if (userParams.ByHourly)
            {
                query = query.Where(x => x.BusinessInfo.HourlyRate >= minPrice && x.BusinessInfo.HourlyRate <= maxPrice);
            }
            
            if (!string.IsNullOrEmpty(userParams.Search))
            {
                query = query.Where(x => x.UserName.Contains(userParams.Search) || x.BusinessInfo.BusinessName.Contains(userParams.Search));
            }

            if (userParams.LanguageIds?.Count != null && userParams.LanguageIds?.Count > 0)
            {
                query = query.Where(x => x.BusinessInfo.SpokenLanguages.Any(l => userParams.LanguageIds.Contains(l.LanguageId)));
            }

            if (userParams.TradeIds?.Count != null && userParams.TradeIds?.Count > 0)
            {
                query = query.Where(x => x.BusinessInfo.Trades.Any(t => userParams.TradeIds.Contains(t.TradeId)));
            }

            if (userParams.MapBoxIds?.Count != null && userParams.MapBoxIds?.Count > 0)
            {
                query = query.Where(x => x.BusinessInfo.Locations.Any(l => userParams.MapBoxIds.Contains(l.Location.MapBoxId)));
            }

            if (userParams.Latitude != null && userParams.Longitude != null)
            {
                //foreach (var q in query)
                //{
                //    q.BusinessInfo.Locations.OrderBy(x => CalculateDistance(x.Location.Longitude, x.Location.Latitude, 
                //        userParams.Longitude.Value, userParams.Latitude.Value));
                //}
                //query = query.OrderBy(x => CalculateDistance(x.BusinessInfo.Locations.FirstOrDefault().Location.Longitude, 
                //                                             x.BusinessInfo.Locations.FirstOrDefault().Location.Latitude,
                //                                            userParams.Longitude.Value, userParams.Latitude.Value));
            }
            //if (spec.Criteria != null)
            //{
            //    query = query.Where(spec.Criteria);
            //}

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }



            //if (spec.IsPagingEnabled)
            //{
            query = query.Skip(spec.Skip).Take(spec.Take);
            //}

            //var distances = query.Select(x => new Point(x.BusinessInfo.Locations.First().Location.Longitude, x.BusinessInfo.Locations.First().Location.Lati
                

            query = query.Include(x => x.Subscription)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Locations)
                        .ThenInclude(x => x.Location)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Trades)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.SpokenLanguages);
            return await query.ToListAsync();
        }

        public static double CalculateDistance(double langitude, double latitude, double newLong, double newLat)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = langitude * (Math.PI / 180.0);
            var d2 = newLat * (Math.PI / 180.0);
            var num2 = newLong * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}