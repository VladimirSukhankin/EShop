using EShopFanerum.Domain.Entites.Shop;

namespace EShopFanerum.Persistance.Repositories.Shop;

public interface IGoodRepository : IBaseRepository<Good, long>
{
    public IQueryable<Good> GetGoodsByIds(List<long> ids);
}