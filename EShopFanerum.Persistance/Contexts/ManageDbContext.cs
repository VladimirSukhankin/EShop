using EShopFanerum.Domain.Entites.Manage;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Persistance.Contexts;

public class ManageDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Document> Documents { get; set; }
    
    public ManageDbContext(DbContextOptions<ManageDbContext> options) : base(options)
    {
    }
}