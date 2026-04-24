using BasketService.Domain.Entities;

namespace BasketService.Domain.Repositories;

public interface IBasketRepository
{
    Task<Basket?> GetBasketAsync(string userId);
    Task<Basket> UpdateBasketAsync(Basket basket);
    Task DeleteBasketAsync(string userId);
}
