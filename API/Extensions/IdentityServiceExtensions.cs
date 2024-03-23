using System.Text;
using Core.Entities;
using Core.Entities.Identity;
using Google.Apis.Util;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
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
                //opt.UseNpgsql(config.Get<MyAwsCredentials>().IdentityConnection);

                opt.UseNpgsql(config.GetConnectionString("IdentityConnection"), x => x.UseNetTopologySuite());
                //opt.UseNpgsql(Environment.GetEnvironmentVariable("IDENTITY_CONNECTION_STRING"));

            });

            services.AddIdentityCore<AppUser>(opt => 
            {
                // add identity options here
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                //options.Password.RequiredUniqueChars = 1;
            });

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));

            services.AddAuthentication(o => {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
                .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
                {
                    var googleAuth = config.GetSection("Authentication:Google");
                    googleOptions.ClientId = googleAuth["ClientId"];
                    googleOptions.ClientSecret = googleAuth["ClientSecret"];
                    googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
                    googleOptions.SaveTokens = true;
                })
                .AddFacebook(FacebookDefaults.AuthenticationScheme, facebookOptions =>
                {
                    var facebookAuth = config.GetSection("Authentication:Facebook");
                    facebookOptions.ClientId = facebookAuth["ClientId"];
                    facebookOptions.ClientSecret = facebookAuth["ClientSecret"];
                });


            services.AddAuthorization();

            return services;
        }
    }
}