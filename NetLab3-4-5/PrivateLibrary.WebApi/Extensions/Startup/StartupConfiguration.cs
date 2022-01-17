using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.BLL.Services.Realizations;
using PrivateLibrary.Common;
using PrivateLibrary.DAL.Contexts;
using PrivateLibrary.DAL.Models.User;
using PrivateLibrary.DAL.Repositories.Interfaces;
using PrivateLibrary.DAL.Repositories.Realizations;
using PrivateLibrary.WebApi.Config;
using PrivateLibrary.WebApi.Config.RequestLimits;

namespace PrivateLibrary.WebApi.Extensions.Startup
{
    internal static class StartupConfiguration
    {
        public static void ConfigureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddCors();
            serviceCollection.AddSession();
            serviceCollection.AddDistributedMemoryCache();
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
            
            serviceCollection.Configure<TokenReqOptions>(configuration.GetSection(TokenReqOptions.Name));
            serviceCollection.AddTransient<ITokenService, TokenService>();
            serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            serviceCollection.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PrivateLibrary Web.Api", Version = "v1" });
            });

            serviceCollection.AddControllers();
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

            app.UseSession();
            
            app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                await next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
