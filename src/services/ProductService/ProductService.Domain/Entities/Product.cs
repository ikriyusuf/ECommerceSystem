namespace ProductService.Domain.Entities;

public class Product : AuditEntity
{
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    // EF Core için parametresiz constructor
    private Product() { }

    /// <summary>
    /// Yeni bir ürün oluşturmak için factory method.
    /// </summary>
    public static Product Create(string name, string description, decimal price, int stock)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        ArgumentOutOfRangeException.ThrowIfNegative(stock);

        return new Product
        {
            Name = name,
            Description = description,
            Price = price,
            Stock = stock,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = "system",
            IsDeleted = false
        };
    }

    /// <summary>
    /// Mevcut ürünü günceller.
    /// </summary>
    public void Update(string name, string description, decimal price, int stock)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        ArgumentOutOfRangeException.ThrowIfNegative(stock);

        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "system";
    }

    /// <summary>
    /// Ürünü soft-delete ile siler.
    /// </summary>
    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedDate = DateTime.UtcNow;
        UpdatedBy = "system";
    }
}
