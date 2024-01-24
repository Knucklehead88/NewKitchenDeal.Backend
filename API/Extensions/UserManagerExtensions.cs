using System.Security.Claims;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                .Include(x => x.PersonalInfo)
                    .ThenInclude(x => x.Location)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindUserByClaimsPrincipleWithBusinessInfo(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Locations)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Trades)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindUserByEmailWithBusinessInfo(this UserManager<AppUser> userManager, string email)
        {
            return await userManager.Users
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Locations)
                .Include(x => x.BusinessInfo)
                    .ThenInclude(x => x.Trades)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this UserManager<AppUser> userManager, 
            ClaimsPrincipal user)
        {
            return await userManager.Users
                .SingleOrDefaultAsync(x => x.Email == user.FindFirstValue(ClaimTypes.Email));
        }
    }
}