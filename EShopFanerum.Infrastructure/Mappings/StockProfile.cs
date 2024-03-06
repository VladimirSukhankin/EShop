using AutoMapper;
using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Infrastructure.Dto;
using EShopFanerum.Infrastructure.Requests;
using EShopFanerum.Infrastructure.Requests.Good;

namespace EShopFanerum.Infrastructure.Mappings;

public class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<Good, GoodDto>()
        .ForMember(dto => dto.CategoryName, o => o.MapFrom(e => e.Category !=null ? e.Category.Name : ""));

        CreateMap<AddGoodRequest, Good>();

        CreateMap<UpdateGoodRequest, Good>();
        
        
    }
    
}