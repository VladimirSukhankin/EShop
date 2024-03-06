using EShopFanerum.Domain.Entites.Materials;

namespace EShopFanerum.Persistance.Repositories.Materials;

public interface ISupplierRepository
{
    IQueryable<Supplier> GetSuppliersWithPaging(int pageIndex, int pageSize);
    IQueryable<Supplier> GetSupplierById(int idSupplier);
}