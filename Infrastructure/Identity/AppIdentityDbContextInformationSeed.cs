using Core.Entities.Identity;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextInformationSeed
    {
        public static async Task SeedTradesAndLanguagesAsync(AppIdentityDbContext appIdentityDbContext)
        {
            if (!appIdentityDbContext.Trades.Any())
            {
                var trades = new List<Trade>()
                {
                    new() { Name = "Electrician" },
                    new() { Name = "Plumber" },
                    new() { Name = "Carpenter" },
                    new() { Name = "Roofer" },
                    new() { Name = "Glazier" },
                    new() { Name = "Tile setter" },
                    new() { Name = "Brick mason" },
                    new() { Name = "Concrete finisher" },
                    new() { Name = "Ironworker" },
                    new() { Name = "Painter" },
                    new() { Name = "Pipefitter" },
                    new() { Name = "Cost estimator" },
                    new() { Name = "Construction manager" },
                    new() { Name = "Safety manager" },
                    new() { Name = "Crane operator" },
                    new() { Name = "Surveyor" },
                    new() { Name = "Flooring installer" },
                    new() { Name = "Construction inspector" },
                };

                await appIdentityDbContext.Trades.AddRangeAsync(trades);
            }

            if (!appIdentityDbContext.Languages.Any())
            {
                var trades = new List<Language>()
                {
                    new() { Name = "English" },
                    new() { Name = "Spanish" },
                    new() { Name = "Chinese" },
                    new() { Name = "Italian" },
                    new() { Name = "French" },
                    new() { Name = "German" },
                    new() { Name = "Japanese" },
                    new() { Name = "Hindi" },
                    new() { Name = "Bengali" },
                    new() { Name = "Turkish" },
                    new() { Name = "Russian" },
                    new() { Name = "Portuguese" },
                    new() { Name = "Arabic" },
                    new() { Name = "Romanian" },
                    new() { Name = "Dutch" },
                };

                await appIdentityDbContext.Languages.AddRangeAsync(trades);
                await appIdentityDbContext.SaveChangesAsync();

            }
        }
    }
}