using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Persistance.Contexts;
using EShopFanerum.Persistance.Repositories.Shop;

namespace EShopFanerum.Persistance.Repositories.Impl.Shop;

public class BonusProgramRepository: BaseRepository<BonusProgram, long>, IBonusProgramRepository
{
    public BonusProgramRepository(StockDbContext context) : base(context)
    {
    }
}