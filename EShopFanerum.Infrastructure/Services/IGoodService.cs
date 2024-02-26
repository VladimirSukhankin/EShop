using EShopFanerum.Infrastructure.Dto;

namespace EShopFanerum.Infrastructure.Services;

public interface IGoodService
{
    Task<ICollection<GoodDto>> GetGoodsWithPaging(PagingDto pagingParams);
}