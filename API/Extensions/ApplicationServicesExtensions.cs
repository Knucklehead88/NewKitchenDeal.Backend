using Amazon.S3;
using Amazon.SimpleEmail;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Stripe;
using Infrastructue.Data;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.Stripe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            // services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            //services.AddDbContext<StoreContext>(opt =>
            //{
            //    opt.UseNpgsql(config.Get<MyAwsCredentials>().DefaultConnection);
            //    opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            //    opt.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));
            //});
            //services.AddSingleton<IConnectionMultiplexer>(c => 
            //{
                //var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis")); //localhost
                //var options = ConfigurationOptions.Parse(config.GetConnectionString("localhost"));
                //return ConnectionMultiplexer.Connect(options);
            //});
            //services.AddScoped<IBasketRepository, BasketRepository>();
            //services.AddScoped<IProductRepository, ProductRepository>();
            //services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IPricesService, PricesService>();
            services.AddScoped<IInvoicesService, InvoicesService>();
            services.AddScoped<ISubscriptionsService, SubscriptionsService>();
            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<IPaymentMethodsService, PaymentMethodsService>();
            services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IOrderService, OrderService>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddDefaultAWSOptions(config.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonSimpleEmailService>();
            services.AddSingleton<IMediaUploadService, MediaUploadService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAwsEmailService, AwsEmailService>();

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