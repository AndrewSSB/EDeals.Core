using EDeals.Core.Application.Interfaces;
using EDeals.Core.Application.Interfaces.Email;
using EDeals.Core.Application.Interfaces.SMS;
using EDeals.Core.Application.Interfaces.UserServices;
using EDeals.Core.Infrastructure.Context;
using EDeals.Core.Infrastructure.Identity.Auth;
using EDeals.Core.Infrastructure.Identity.Repository;
using EDeals.Core.Infrastructure.Seeders;
using EDeals.Core.Infrastructure.Services.EmailServices;
using EDeals.Core.Infrastructure.Services.SmsService;
using EDeals.Core.Infrastructure.Services.UserServices;
using EDeals.Core.Infrastructure.Settings;
using EDeals.Core.Infrastructure.Shared.CustomProviders;
using EDeals.Core.Infrastructure.Shared.DateTimeHelpers;
using EDeals.Core.Infrastructure.Shared.ExecutionContext;
using EDeals.Core.Infrastructure.TokenHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EDeals.Core.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddDbContext(this IServiceCollection services, DbSettings dbSettings)
        {
            var connectionString = dbSettings.MySqlConnectionString;
            var db_username = dbSettings.Username;
            var db_password = dbSettings.Password;
            var db_endpoint = dbSettings.Endpoint.Split(":")[0];
            var db_port = dbSettings.Endpoint.Split(":")[1];
            var db_max_pool_size = dbSettings.MaxPoolSize;

            connectionString = string.Format(connectionString, db_endpoint, db_port, db_username, db_password, db_max_pool_size);

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString,
                    new MySqlServerVersion(new Version(8, 0, 32)),
                    options => options.EnableRetryOnFailure()
                )
            );
        }

        public static IServiceCollection AddInfrastructureMethods(this IServiceCollection services)
        {
            // Services
            services.AddTransient<IIdentityBaseRepository, IdentityRepository>();
            services.AddTransient<IIdentityRepository, IdentityRepository>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ITokenHelper, TokenHelper>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IDateTimeHelper, DateTimeHelper>();
            services.AddSingleton<ICustomExecutionContext, CustomExecutionContext>();

            services.AddScoped<RefreshTokenProvider<ApplicationUser>>();

            // APIs
            services.AddTransient<IBaseEmailService, BaseEmailService>();
            services.AddTransient<ISendSmsService, SendSmsService>();
            
            // Seeders
            services.AddScoped<IdentityRoleSeeder>();

            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.ProviderMap.Add("RefreshTokenProvider", new TokenProviderDescriptor(typeof(RefreshTokenProvider<ApplicationUser>)));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true;
                //opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.Zero;
                opt.Lockout.MaxFailedAccessAttempts = 10;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
            services.Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));
            services.Configure<TwilioSettings>(configuration.GetSection(nameof(TwilioSettings)));
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

            return services;
        }
    }
}
