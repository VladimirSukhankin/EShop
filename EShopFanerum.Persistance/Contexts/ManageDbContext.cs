using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Persistance.Contexts;

public class ManageDbContext : DbContext
{
    
    public ManageDbContext(DbContextOptions<StockDbContext> options) : base(options)
    {
    }
}