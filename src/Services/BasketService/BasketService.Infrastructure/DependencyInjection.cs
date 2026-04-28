using BasketService.Domain.Repositories;
using BasketService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string redisConnectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        services.AddScoped<IBasketRepository, RedisBasketRepository>();

        return services;
    }
}
