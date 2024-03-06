using EShopFanerum.Avalonia.ManagerApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Avalonia.ManagerApp.Persistence;

public class TestDbContext : DbContext
{
    private DbSet<TestEntity> TestEntities { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User ID=postgres;Database=TESTCONTEXT;Password=1;TrustServerCertificate=True");
    }
}