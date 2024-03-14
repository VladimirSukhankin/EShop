using EShopFanerum.Persistance.Contexts;
using EShopFanerum.Persistance.Repositories.Material;

namespace EShopFanerum.Persistance.Repositories.Impl.Material;

public class MaterialRepository : BaseRepository<Domain.Entites.Materials.Material, long>, IMaterialRepository
{
    public MaterialRepository(StockDbContext context) : base(context)
    {
    }
}