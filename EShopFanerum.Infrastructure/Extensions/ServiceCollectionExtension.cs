using EShopFanerum.Infrastructure.Services;
using EShopFanerum.Infrastructure.Services.Impl;
using EShopFanerum.Persistance.Exstension;
using EShopFanerum.Persistance.Repositories.Impl.Shop;
using EShopFanerum.Persistance.Repositories.Shop;
using Microsoft.Extensions.DependencyInjection;

namespace EShopFanerum.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddStockServices(this IServiceCollection services)
    {
        services.AddScoped<IGoodRepository, GoodRepository>();
        services.AddScoped<ISupportService, SupportService>();
        services.AddScoped<IGoodService, GoodService>();
        
        services.AddStockDbContext();
    }
}