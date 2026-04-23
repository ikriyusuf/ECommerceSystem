using System.Text.Json;
using BasketService.Domain.Entities;
using BasketService.Domain.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace BasketService.Infrastructure.Repositories;

public class RedisBasketRepository : IBasketRepository
{
    private readonly IDistributedCache _cache;

    public RedisBasketRepository(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<Basket?> GetBasketAsync(Guid id)
    {
        var basket = await _cache.GetStringAsync(id.ToString());

        if (string.IsNullOrEmpty(basket))
            return null;

        return JsonSerializer.Deserialize<Basket>(basket);
    }

    public async Task<Basket> UpdateBasketAsync(Basket basket)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
        };

        await _cache.SetStringAsync(
            basket.Id.ToString(),
            JsonSerializer.Serialize(basket),
            options
        );

        return basket;
    }

    public async Task DeleteBasketAsync(string id)
    {
        await _cache.RemoveAsync(id);
    }
}
