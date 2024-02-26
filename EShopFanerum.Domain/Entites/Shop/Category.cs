namespace EShopFanerum.Domain.Entites.Shop;

public class Category
{
    public long Id { get; set; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
}