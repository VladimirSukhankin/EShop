namespace EShopFanerum.Domain.Entites.Shop;

public class Good : IEntity<long>
{
    public long Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public long CategoryId { get; set; }
    
    public decimal Price { get; set; }
    
    public long? ImageId { get; set; }
    
    public long? ImageIconId { get; set; }
    
    public Category? Category { get; set; }

    public long QuantityInStock { get; set; }
}