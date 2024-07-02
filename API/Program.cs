using Amazon.S3;
using API.Extensions;
using API.Middleware;
using Core.Entities;
using Core.Entities.Identity;
using Infrastructue.Data;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using static Org.BouncyCastle.Math.EC.ECCurve;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Configuration.AddAmazonSecretsManager("us-east-1", "Development_NewProjectDeal.Backend");
builder.Services.Configure<MyAwsCredentials>(configuration);

builder.Services.AddScoped(config =>  config.GetService<IOptions<MyAwsCredentials>>().Value);

builder.Services.AddApplicationServices(configuration);
builder.Services.AddIdentityServices(configuration);
// builder.Services.AddStripe(builder.Configuration);
builder.Services.AddSwaggerDocumentation();



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseSwaggerDocumentation();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Content")),
    RequestPath = "/Content"
});

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
//var context = services.GetRequiredService<StoreContext>();
var identityContext = services.GetRequiredService<AppIdentityDbContext>();
var userManager = services.GetRequiredService<UserManager<AppUser>>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    //await context.Database.MigrateAsync();
    await identityContext.Database.MigrateAsync();
    //await StoreContextSeed.SeedAsync(context);
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
    await AppIdentityDbContextInformationSeed.SeedTradesAndLanguagesAsync(identityContext);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
