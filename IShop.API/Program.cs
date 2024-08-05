using IShop.API;
using IShop.API.Database;
using IShop.API.Database.Entities.Security;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        var userManager = service.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = service.GetRequiredService<RoleManager<RoleEntity>>();
        var dbContext = service.GetRequiredService<IShopDbContext>();

        await IShopDbSeeder.LoadDataAsync(userManager, roleManager, loggerFactory, dbContext);
    }
    catch (Exception e)
    {
        var logger = loggerFactory.CreateLogger<IShopDbSeeder>();
        logger.LogError(e, "Error al inicializar datos con Seed.");
    }
}

app.Run();