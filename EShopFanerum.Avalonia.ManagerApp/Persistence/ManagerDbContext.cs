using EShopFanerum.Avalonia.ManagerApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Avalonia.ManagerApp.Persistence;

public class ManagerDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<Document> Documents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User ID=postgres;Database=manager;Password=1;TrustServerCertificate=True");
    }
}