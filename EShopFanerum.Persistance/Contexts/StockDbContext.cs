using EShopFanerum.Domain.Entites.Materials;
using EShopFanerum.Domain.Entites.Shop;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Persistance.Contexts;

public class StockDbContext : DbContext
{
    public DbSet<Good> Goods { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BonusProgram> BonusPrograms { get; set; }
    public DbSet<BonusProgramGoods> BonusProgramGoods { get; set; }
    public DbSet<BonusProgramGoods> ShopingCarts { get; set; }
    
    public DbSet<Material> Materials { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<MaterialSupplier> MaterialSuppliers { get; set; }
    

    public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
    {
    }
}