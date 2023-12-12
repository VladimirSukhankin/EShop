namespace EShopFanerum.Domain.Entites.Shop;

public class Good
{
    public long Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Category Category { get; set; } = null!;

    public decimal Price { get; set; }

    public long? ImageId { get; set; }

    public ICollection<long>? ImageDetailId { get; set; }

    public long? ImageIconId { get; set; }
}