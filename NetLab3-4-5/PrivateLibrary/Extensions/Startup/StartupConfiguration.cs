using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.BLL.Services.Realizations;
using PrivateLibrary.Common;
using PrivateLibrary.Config.RequestLimits;
using PrivateLibrary.DAL.Contexts;
using PrivateLibrary.DAL.Models.User;
using PrivateLibrary.DAL.Repositories.Interfaces;
using PrivateLibrary.DAL.Repositories.Realizations;

namespace PrivateLibrary.Extensions.Startup
{
    internal static class StartupConfiguration
    {
        public static void ConfigureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            serviceCollection.AddDbContext<IdentityDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("IdentityConnection"), b => b.MigrationsAssembly(AssemblyNames.PrivateLibraryDal)));
            serviceCollection.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            serviceCollection.AddDbContext<LibraryDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("LibraryConnection"), b => b.MigrationsAssembly(AssemblyNames.PrivateLibraryDal)));

            serviceCollection.AddTransient<IBookRepository, BookRepository>();
            serviceCollection.AddTransient<IAuthorRepository, AuthorRepository>();
            serviceCollection.AddTransient<IDirectionRepository, DirectionRepository>();
            serviceCollection.AddTransient<ICustomerBookRepository, CustomerBookRepository>();

            serviceCollection.AddTransient<IAuthService, AuthService>();
            serviceCollection.AddTransient<IAccountManagerService, AccountManagerService>();
            serviceCollection.AddTransient<IBookManagerService, BookManagerService>();
            serviceCollection.AddTransient<IAuthorService, AuthorService>();
            serviceCollection.AddTransient<IDirectionService, DirectionService>();
            serviceCollection.AddTransient<ICustomerBookService, CustomerBookService>();

            serviceCollection.Configure<BookLimits>(configuration.GetSection(BookLimits.Name));

            serviceCollection.AddControllersWithViews();
        }

        public static void Configure(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("uk"),
            };

            var requestLocalization = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };

            app.UseRequestLocalization(requestLocalization);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
