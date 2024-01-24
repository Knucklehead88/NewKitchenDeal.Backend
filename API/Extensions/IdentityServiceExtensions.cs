using System.Text;
using Core.Entities.Identity;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, 
            IConfiguration config)
        {
            services.AddDbContext<AppIdentityDbContext>(opt =>
            {
                // opt.UseNpgsql(config.GetConnectionString("IdentityConnection"));
                var env = Environment.GetEnvironmentVariable("IDENTITY_CONNECTION_STRING");
                if (string.IsNullOrEmpty(env)) {
                    Environment.SetEnvironmentVariable("IDENTITY_CONNECTION_STRING", "host=db;port=5432;username=appuser;password=secret;database=identity");
                }
                opt.UseNpgsql(Environment.GetEnvironmentVariable("IDENTITY_CONNECTION_STRING"));

            });

            services.AddIdentityCore<AppUser>(opt => 
            {
                // add identity options here
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        ValidIssuer = config["Token:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                })
                .AddGoogle("google", googleOptions =>
                {
                    var googleAuth = config.GetSection("Authentication:Google");
                    googleOptions.ClientId = googleAuth["ClientId"];
                    googleOptions.ClientSecret = googleAuth["ClientSecret"];
                    googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                });


            services.AddAuthorization();

            return services;
        }
    }
}