namespace Shared.Contracts.Events;

public interface IAddToBasketEvent
{
    string UserId { get; }
    Guid ProductId { get; }
    string ProductName { get; }
    decimal Price { get; }
    int Quantity { get; }
}
