using Microsoft.Extensions.DependencyInjection;
using unified_customer_profile.Api;
using unified_customer_profile.Repository;
using unified_customer_profile.Service;
using unified_customer_profile.Shared;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();