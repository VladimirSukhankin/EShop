using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Persistance.Contexts;
using EShopFanerum.Persistance.Repositories.Shop;

namespace EShopFanerum.Persistance.Repositories.Impl.Shop;

public class GoodRepository : BaseRepository<Good, long>, IGoodRepository
{
    private readonly StockDbContext _stockDbContext;
    public GoodRepository(StockDbContext context) : base(context)
    {
        _stockDbContext = context;
    }

    public IQueryable<Good> GetGoodsByIds(List<long> ids)
    {
        return _stockDbContext.Goods.Where(x => ids.Contains(x.Id));
    }
}