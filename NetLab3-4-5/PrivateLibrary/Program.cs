using Microsoft.AspNetCore.Identity;
using PrivateLibrary.DAL.Models.User;
using PrivateLibrary.DAL.Utils.Initializations;
using PrivateLibrary.Extensions.Startup;

var builder = WebApplication.CreateBuilder(args);

// Configuring services
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
app.Configure();

app.Run();
