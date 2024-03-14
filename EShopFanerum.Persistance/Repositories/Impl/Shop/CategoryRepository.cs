using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Persistance.Contexts;
using EShopFanerum.Persistance.Repositories.Shop;

namespace EShopFanerum.Persistance.Repositories.Impl.Shop;

public class CategoryRepository : BaseRepository<Category, long>, ICategoryRepository
{
    public CategoryRepository(StockDbContext context) : base(context)
    {
    }
}