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

    public async Task<Basket?> GetBasketAsync(string userId)
    {
        var data = await _cache.GetStringAsync(userId);

        if (string.IsNullOrEmpty(data))
            return null;

        return JsonSerializer.Deserialize<Basket>(data);
    }

    public async Task<Basket> UpdateBasketAsync(Basket basket)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
        };

        await _cache.SetStringAsync(
            basket.UserId,
            JsonSerializer.Serialize(basket),
            options
        );

        return basket;
    }

    public async Task DeleteBasketAsync(string userId)
    {
        await _cache.RemoveAsync(userId);
    }
}
