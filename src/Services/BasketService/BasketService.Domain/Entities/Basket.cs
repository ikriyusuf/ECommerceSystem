namespace BasketService.Domain.Entities;

public class Basket
{
    public string UserId { get; set; } = string.Empty;
    public List<BasketItem> Items { get; set; } = new();

    /// <summary>
    /// Sepetteki tüm ürünlerin genel toplam fiyatı
    /// </summary>
    public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
}
