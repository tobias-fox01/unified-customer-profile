using Microsoft.Extensions.DependencyInjection;
using Serilog;
using unified_customer_profile.Api;
using unified_customer_profile.Repository;
using unified_customer_profile.Service;
using unified_customer_profile.Shared;

// Creates a temporary logger that captures any errors during startup
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting server.");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog logging
    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services));

    // Add configuration from app settings
    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

    // Add automapper
    builder.Services.InitalizeAutoMapper();

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddApplicationConfig(builder.Configuration);
    builder.Services.AddMiddlewareRepositories();
    builder.Services.AddMiddlewareServices();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
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
catch (Exception ex)
{
    Log.Fatal(ex, "Server terminated unexpectedly.");
}
finally
{
    // Guarantees all buffered log events are written before the process exits
    Log.CloseAndFlush();
}