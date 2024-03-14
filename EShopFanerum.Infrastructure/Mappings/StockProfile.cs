using AutoMapper;
using EShopFanerum.Domain.Entites.Materials;
using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Infrastructure.Dto.Shop;
using EShopFanerum.Infrastructure.Dto.Stock;

namespace EShopFanerum.Infrastructure.Mappings;

public class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<Good, GoodDto>()
        .ForMember(dto => dto.CategoryName, o => o.MapFrom(e => e.Category !=null ? e.Category.Name : ""));
        
        CreateMap<GoodDto, Good>();
        
        CreateMap<Material, MaterialDto>();
        CreateMap<MaterialDto, Material>();
        
        CreateMap<Supplier, SupplierDto>();
        CreateMap<SupplierDto, Supplier>();
    }
    
}