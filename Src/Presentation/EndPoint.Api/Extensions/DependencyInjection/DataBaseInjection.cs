using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace EndPoint.Api.Extensions.DependencyInjection;

public static class DataBaseInjection
{
    public static IServiceCollection AddConfiguredDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection")!;

        services
            .AddDbContext<DataBaseContext>(option => option
                .UseSqlServer(defaultConnection,
                    providerOptions =>
                    {
                        providerOptions.CommandTimeout(int.MaxValue); //Timeout in seconds
                    }));

        services.AddScoped<IDapperContext>(_ => new DapperContext(defaultConnection));
        services.AddScoped<IDataBaseContext, DataBaseContext>();

        return services;
    }
}