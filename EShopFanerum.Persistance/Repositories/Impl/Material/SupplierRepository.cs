using EShopFanerum.Domain.Entites.Materials;
using EShopFanerum.Persistance.Contexts;
using EShopFanerum.Persistance.Repositories.Material;

namespace EShopFanerum.Persistance.Repositories.Impl.Material;

public class SupplierRepository : BaseRepository<Supplier, long>, ISupplierRepository
{
    public SupplierRepository(StockDbContext context) : base(context)
    {
    }
}