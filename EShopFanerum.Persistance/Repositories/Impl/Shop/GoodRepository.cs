using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Persistance.Contexts;
using EShopFanerum.Persistance.Repositories.Shop;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Persistance.Repositories.Impl;

public class GoodRepository : IGoodRepository
{
    private readonly StockDbContext _stockDbContext;
    
    public GoodRepository(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }
    public IQueryable<Good> GetGoodsWithPaging(int pageIndex, int pageSize)
    {
        return _stockDbContext.Goods
            .AsNoTracking()
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }

    public IQueryable<Good> GetGoodById(int idGood)
    {
        return _stockDbContext.Goods
            .AsNoTracking()
            .Where(x=>x.Id == idGood);
    }
}