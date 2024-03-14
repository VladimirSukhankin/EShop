namespace EShopFanerum.Domain.Entites.Materials;

public class Material : IEntity<long>
{
    public long Id { get; set; }
    
    public required string Name { get; set; }

    public required long Count { get; set; }
}