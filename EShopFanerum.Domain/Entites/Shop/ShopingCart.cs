namespace EShopFanerum.Domain.Entites.Shop;

public class ShopingCart
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long GoodId { get; set; }

    public long CountGoods { get; set; }
}