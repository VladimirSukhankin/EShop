using EShopFanerum.Domain.Entites.Materials;

namespace EShopFanerum.Persistance.Repositories.Materials;

public interface IMaterialRepository
{
    IQueryable<Material> GetMaterialsWithPaging(int pageIndex, int pageSize);
    IQueryable<Material> GetMaterialById(int idMaterial);
}