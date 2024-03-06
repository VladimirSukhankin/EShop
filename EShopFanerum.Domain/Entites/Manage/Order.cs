using EShopFanerum.Domain.Enums;

namespace EShopFanerum.Domain.Entites.Manage;

//Заказы
public class Order
{
    public long Id { get; set; }
    
    public long[] GoodIds { get; set; }

    public decimal Price { get; set; }

    public StateOrder StateOrder { get; set; }

    public DateTime CreatedDateTime { get; set; }
}