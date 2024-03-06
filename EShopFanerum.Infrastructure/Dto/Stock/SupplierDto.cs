namespace EShopFanerum.Infrastructure.Dto.Stock;

public class SupplierDto
{
    public long Id { get; set; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }

    public string Phone { get; set; }

    public bool IsActive { get; set; }

    public byte Raiting { get; set; }
}