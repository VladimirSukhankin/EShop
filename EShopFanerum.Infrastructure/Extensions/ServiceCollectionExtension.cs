using EShopFanerum.Persistance.Exstension;
using EShopFanerum.Persistance.Repositories;
using EShopFanerum.Persistance.Repositories.Impl;
using EShopFanerum.Persistance.Repositories.Shop;
using Microsoft.Extensions.DependencyInjection;

namespace EShopFanerum.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddStockServices(this IServiceCollection services)
    {
        services.AddScoped<IGoodRepository, GoodRepository>();
        services.AddStockDbContext();
        services.AddManageDbContext();
    }
}