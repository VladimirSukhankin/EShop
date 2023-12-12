using EShopFanerum.Domain.Entites.Shop;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Persistance.Contexts;

public class DataContext : DbContext
{
    public DbSet<Good> Goods { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CardItem> CardItems { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}