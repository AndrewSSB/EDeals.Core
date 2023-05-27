using EDeals.Core.API;
using EDeals.Core.Application;
using EDeals.Core.Infrastructure;
using EDeals.Core.Infrastructure.Context;
using EDeals.Core.Infrastructure.Seeders;
using EDeals.Core.Infrastructure.Settings;
using EDeals.Core.Infrastructure.Shared.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting application...");

    var builder = WebApplication.CreateBuilder(args);

    // Add configurations
    ApiExtensions.AddApplicationSettings(builder);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add Services
    var dbSettings = builder.Configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();

    builder.Services.AddDbContext(dbSettings!);

    builder.Services
        .AddApplicationMethods()
        .AddInfrastructureMethods()
        .ConfigureSettings(builder.Configuration)
        .AddCustomIdentity();

    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    // Add Logging
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    var app = builder.Build();

    RunMigrations(app);
    await RunSeeders(app);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (ex is not OperationCanceledException && !ex.GetType().Name.Contains("StopTheHostException") && !ex.GetType().Name.Contains("HostAbortedException"))
{
    Log.Fatal(ex, "Unhandled exception in Program");
}
finally
{
    Log.Information("App shut down complete");
    Log.CloseAndFlush();
}

void RunMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dataContext.Database.Migrate();
}

async Task RunSeeders(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var roleSeeders = scope.ServiceProvider.GetRequiredService<IdentityRoleSeeder>();
    await roleSeeders.CreateRoles();
}