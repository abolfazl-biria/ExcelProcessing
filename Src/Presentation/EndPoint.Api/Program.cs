using EndPoint.Api.Extensions.DependencyInjection;
using EndPoint.Api.Extensions.Middleware;
using EndPoint.Api.Extensions.Validations;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MemoryStorage;
using OfficeOpenXml;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog().ConfigureLogging(loggingConfiguration => loggingConfiguration.ClearProviders());

var services = builder.Services;
var configuration = builder.Configuration;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

services
    .AddConfiguredDatabase(configuration)
    .AddServices()
    .AddConfiguredSwagger();

services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});
services.AddHangfireServer();

services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<UploadExcelRequestValidator>());

services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseConfiguredSwagger();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseConfiguredExceptionHandler(builder.Environment);

app.MapControllers();
app.UseHangfireDashboard();

app.Run();