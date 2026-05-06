using BasketService.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure;

public static class MassTransitRegistration
{
    public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            // Consumer'ları ekle
            x.AddConsumer<AddToBasketConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"] ?? "localhost", h =>
                {
                    h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                    h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                });

                // Endpoint konfigürasyonları
                cfg.ReceiveEndpoint("basket-add-to-basket-queue", e =>
                {
                    e.ConfigureConsumer<AddToBasketConsumer>(context);
                });
            });
        });

        return services;
    }
}
