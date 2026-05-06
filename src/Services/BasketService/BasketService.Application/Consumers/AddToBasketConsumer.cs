using BasketService.Domain.Entities;
using BasketService.Domain.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Events;

namespace BasketService.Application.Consumers;

public class AddToBasketConsumer : IConsumer<IAddToBasketEvent>
{
    private readonly IBasketRepository _repository;
    private readonly ILogger<AddToBasketConsumer> _logger;

    public AddToBasketConsumer(IBasketRepository repository, ILogger<AddToBasketConsumer> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IAddToBasketEvent> context)
    {
        _logger.LogInformation("Add to basket event received. UserId: {UserId}, ProductId: {ProductId}", 
            context.Message.UserId, context.Message.ProductId);

        var basket = await _repository.GetBasketAsync(context.Message.UserId);

        if (basket is null)
        {
            basket = new Basket { UserId = context.Message.UserId };
        }

        var existingItem = basket.Items.FirstOrDefault(x => x.ProductId == context.Message.ProductId.ToString());

        if (existingItem != null)
        {
            existingItem.Quantity += context.Message.Quantity;
        }
        else
        {
            basket.Items.Add(new BasketItem
            {
                ProductId = context.Message.ProductId.ToString(),
                ProductName = context.Message.ProductName,
                Price = context.Message.Price,
                Quantity = context.Message.Quantity
            });
        }

        await _repository.UpdateBasketAsync(basket);
        
        _logger.LogInformation("Basket updated successfully for user: {UserId}", context.Message.UserId);
    }
}
