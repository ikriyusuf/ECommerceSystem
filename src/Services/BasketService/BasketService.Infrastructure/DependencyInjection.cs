using BasketService.Domain.Repositories;
using BasketService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BasketService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        services.AddScoped<IBasketRepository, RedisBasketRepository>();

        // MassTransit (RabbitMQ)
        services.AddMassTransitWithRabbitMQ(configuration);

        return services;
    }
}
