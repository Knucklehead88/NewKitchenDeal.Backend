using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using API.Dtos;
using Core.Entities.Identity;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            if (userParams?.DailyRate != 0)
            {
                query = query.Where(x => x.BusinessInfo.DailyRate == userParams.DailyRate);
            }

            if (userParams?.HourlyRate != 0)
            {
                query = query.Where(x => x.BusinessInfo.HourlyRate == userParams.HourlyRate);
            }

            if (!string.IsNullOrEmpty(userParams.DailyRateRange))
            {
                decimal lowerRange = decimal.MinValue;
                decimal upperRange = decimal.MaxValue;
                var result = userParams.DailyRateRange.Split('-').Select(d => d.Trim());
                _ = decimal.TryParse(result.First(), out lowerRange);
                _ = decimal.TryParse(result.Last(), out upperRange);
                query = query.Where(x => x.BusinessInfo.DailyRate > lowerRange && x.BusinessInfo.DailyRate < upperRange);
            }

            if (!string.IsNullOrEmpty(userParams.HourlyRateRange))
            {
                decimal lowerRange = decimal.MinValue;
                decimal upperRange = decimal.MaxValue;
                var result = userParams.HourlyRateRange.Split('-').Select(d => d.Trim());
                _ = decimal.TryParse(result.First(), out lowerRange);
                _ = decimal.TryParse(result.Last(), out upperRange);
                query = query.Where(x => x.BusinessInfo.HourlyRate >= lowerRange && x.BusinessInfo.HourlyRate <= upperRange);
            }

            if (!string.IsNullOrEmpty(userParams.Search))
            {
                query = query.Where(x => x.UserName.Contains(userParams.Search) || x.BusinessInfo.BusinessName.Contains(userParams.Search));
            }

            if (userParams.LanguageId.HasValue)
            {
                query = query.Where(x => x.BusinessInfo.SpokenLanguages.Any(l => l.LanguageId == userParams.LanguageId));
            }

            if (userParams.TradeId.HasValue)
            {
                query = query.Where(x => x.BusinessInfo.Trades.Any(t => t.TradeId == userParams.TradeId));
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
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
                
            //    tude));

            query = query.Include(x => x.Subscription)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Locations)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Trades)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.SpokenLanguages);
            return await query.ToListAsync();
        }
    }
}