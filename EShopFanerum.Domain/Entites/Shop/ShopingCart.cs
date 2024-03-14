namespace EShopFanerum.Domain.Entites.Shop;

public class ShopingCart : IEntity<long>
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long GoodId { get; set; }

    public long CountGoods { get; set; }
}