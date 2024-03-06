namespace EShopFanerum.Infrastructure.Dto.Stock;

public class MaterialDto
{
    public long Id { get; set; }
    
    public required string Name { get; set; }

    public required long Count { get; set; }
}