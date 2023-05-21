using EDeals.Core.Infrastructure.Context;
using EDeals.Core.Infrastructure.Identity.Auth;
using EDeals.Core.Infrastructure.Identity.Repository;
using EDeals.Core.Infrastructure.Seeders;
using EDeals.Core.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransient<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IdentityRoleSeeder>();

            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
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

            return services;
        }
    }
}
