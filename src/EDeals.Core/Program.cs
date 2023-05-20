using EDeals.Core.Application;
using EDeals.Core.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add Services
    builder.Services
        .AddApplicationMethods()
        .AddInfrastructureMethods();

    // Add Logging
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

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