using EShopFanerum.Domain.Entites.Shop;

namespace EShopFanerum.Persistance.Repositories.Shop;

public interface IGoodRepository
{
    IQueryable<Good> GetGoodsWithPaging(int pageIndex, int pageSize);
    IQueryable<Good> GetGoodById(int idGood);
}