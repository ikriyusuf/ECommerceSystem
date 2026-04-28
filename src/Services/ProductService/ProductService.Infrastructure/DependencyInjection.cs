using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // PostgreSQL bağlantısı
        services.AddDbContext<ProductDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    // Specify migrations assembly
                    npgsqlOptions.MigrationsAssembly(typeof(ProductDbContext).Assembly.FullName);
                    // Enable a retry strategy for transient failures (network/blips from Docker/Postgres)
                    // Adjust maxRetryCount and maxRetryDelay as needed.
                    npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null);
                }));

        // Repository ve Unit of Work kayıtları
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
