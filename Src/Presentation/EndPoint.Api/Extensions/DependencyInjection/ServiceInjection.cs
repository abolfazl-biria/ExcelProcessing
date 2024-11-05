using Application.Interfaces.Products.Commands;
using Application.Services.Products.Commands;

namespace EndPoint.Api.Extensions.DependencyInjection;

public static class ServiceInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddScoped<IAddProductService, AddBulkProductsService>()
            .AddScoped<IProcessingProductService, ProcessingProductService>();
}