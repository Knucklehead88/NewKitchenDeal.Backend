using API.Errors;
using Core.Interfaces;
using Infrastructue.Data;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
                // var env = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                // if (string.IsNullOrEmpty(env)) {
                //     Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", "host=db;port=5432;username=appuser;password=secret;database=skinet");
                // }
                // opt.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));

            });
            services.AddSingleton<IConnectionMultiplexer>(c => 
            {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis")); //localhost
                //var options = ConfigurationOptions.Parse(config.GetConnectionString("localhost"));
                return ConnectionMultiplexer.Connect(options);
            });
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            return services;
        }
    }
}