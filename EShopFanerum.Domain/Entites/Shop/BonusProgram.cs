namespace EShopFanerum.Domain.Entites.Shop;

public class BonusProgram : IEntity<long>
{
    public long Id { get; set; }

    public required string Name { get; set; }
    
    public string? Description { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }
}