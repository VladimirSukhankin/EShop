using AutoMapper;
using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Infrastructure.Dto;

namespace EShopFanerum.Infrastructure.Mappings;

public class StockProfile : Profile
{
    public StockProfile()
    {
        CreateMap<Good, GoodDto>();
        //.ForMember(dto => dto.CategoryName, o => o.MapFrom(e => e.Category.Name));
    }
    
}