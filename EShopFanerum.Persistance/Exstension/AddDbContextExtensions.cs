using EShopFanerum.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EShopFanerum.Persistance.Exstension;

public static class AddDbContextExtensions
{
    public static void AddStockDbContext(this IServiceCollection services)
    {
        services.AddDbContext<StockDbContext>(options =>
            options.UseNpgsql("Server=localhost;Port=5432;User ID=postgres;Database=stock;Password=1;TrustServerCertificate=True"));
    }

    public static void AddManageDbContext(this IServiceCollection services)
    {
        services.AddDbContext<StockDbContext>(options =>
            options.UseNpgsql("Server=localhost;Port=5432;User ID=postgres;Database=manage;Password=1;TrustServerCertificate=True"));
    }
}