using EShopFanerum.Domain.Entites.Auth;

namespace EShopFanerum.Domain.Entites.Shop;

public class CardItem
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public User User { get; set; } = null!;

    public ICollection<Good>? Goods { get; set; }
}