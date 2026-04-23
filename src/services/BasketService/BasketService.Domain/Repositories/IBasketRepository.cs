using BasketService.Domain.Entities;

namespace BasketService.Domain.Repositories;

public interface IBasketRepository
{
    Task<Basket?> GetBasketAsync(Guid id);
    Task<Basket> UpdateBasketAsync(Basket basket);
    Task DeleteBasketAsync(string id);
}
