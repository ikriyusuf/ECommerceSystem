namespace BasketService.Domain.Entities;

public class BasketItem
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    /// <summary>
    /// Bu ürünün sepetteki toplam fiyatı (Price × Quantity)
    /// </summary>
    public decimal TotalPrice => Price * Quantity;
}
