using System.Text;
using Core.Entities.Identity;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, 
            IConfiguration config)
        {
            services.AddDbContext<AppIdentityDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("IdentityConnection"));
                //opt.UseNpgsql(Environment.GetEnvironmentVariable("IDENTITY_CONNECTION_STRING"));

            });

            services.AddIdentityCore<AppUser>(opt => 
            {
                // add identity options here
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager<SignInManager<AppUser>>();

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