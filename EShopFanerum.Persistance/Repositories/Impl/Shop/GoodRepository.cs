using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Persistance.Contexts;
using EShopFanerum.Persistance.Repositories.Shop;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Persistance.Repositories.Impl.Shop;

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
            .Include(x=>x.Category)
            .AsNoTracking()
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }

    public IQueryable<Good> GetGoodById(int idGood)
    {
        return _stockDbContext.Goods
            .Include(x=>x.Category)
            .AsNoTracking()
            .Where(x=>x.Id == idGood);
    }

    public async Task AddGoodAsync(Good good, CancellationToken cancellationToken)
    {
        try
        {
            _stockDbContext.Goods.Add(good);
            await _stockDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task UpdateGoodAsync(Good good, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteGoodAsync(long idGood, CancellationToken cancellationToken)
    {
        var goodDeleted = await _stockDbContext.Goods.SingleOrDefaultAsync(x => x.Id == idGood, cancellationToken);
        if (goodDeleted == null)
        {
            throw new Exception(); //TODO: new Exception NotFoundGood
        }

        _stockDbContext.Remove(goodDeleted);
        await _stockDbContext.SaveChangesAsync(cancellationToken);
    }
}