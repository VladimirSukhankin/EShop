using EShopFanerum.Domain.Entites.Materials;
using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Infrastructure.Dto.Shop;
using EShopFanerum.Infrastructure.Dto.Stock;
using EShopFanerum.Infrastructure.Services;
using EShopFanerum.Infrastructure.Services.Impl;
using EShopFanerum.Persistance.Exstension;
using EShopFanerum.Persistance.Repositories;
using EShopFanerum.Persistance.Repositories.Impl.Material;
using EShopFanerum.Persistance.Repositories.Impl.Shop;
using EShopFanerum.Persistance.Repositories.Material;
using EShopFanerum.Persistance.Repositories.Shop;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace EShopFanerum.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddStockServices(this IServiceCollection services)
    {
        services.AddTransient<IGoodRepository, GoodRepository>();
        services.AddTransient<IGoodService, GoodService>();
        
        services.AddTransient<IMaterialRepository, MaterialRepository>();
        services.AddTransient<IMaterialService, MaterialService>();
        
        services.AddTransient<ISupplierRepository, SupplierRepository>();
        services.AddTransient<ISupplierService, SupplierService>();
        
        
        services.AddTransient<IGenericService<GoodDto, long>, GoodService>();
        services.AddTransient<IGenericService<MaterialDto, long>, MaterialService>();
        services.AddTransient<IGenericService<SupplierDto, long>, SupplierService>();
        
        services.AddTransient<IBaseRepository<Good, long>, GoodRepository>();
        services.AddTransient<IBaseRepository<Material, long>, MaterialRepository>();
        services.AddTransient<IBaseRepository<Supplier, long>, SupplierRepository>();
        
        services.AddTransient<ISupportService, SupportService>();
        
        
        services.AddStockDbContext();
    }
}