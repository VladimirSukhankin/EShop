namespace EShopFanerum.Domain.Entites.Shop;

public class BonusProgramGoods : IEntity<long>
{
    public long Id { get; set; }
    
    public long BonusProgramId { get; set; }
    
    public long GoodId { get; set; }

    public long SalePrice { get; set; }
}