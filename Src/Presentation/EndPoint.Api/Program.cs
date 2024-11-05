using EndPoint.Api.Extensions.DependencyInjection;
using EndPoint.Api.Extensions.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog().ConfigureLogging(loggingConfiguration => loggingConfiguration.ClearProviders());

var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddConfiguredDatabase(configuration)
    .AddConfiguredSwagger();

services.AddControllers();

services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseConfiguredSwagger();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseConfiguredExceptionHandler(builder.Environment);

app.MapControllers();

app.Run();