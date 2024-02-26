using EShopFanerum.Infrastructure.Dto;

namespace EShopFanerum.Infrastructure.Services.Impl;

public class GoodService : IGoodService
{
    public Task<ICollection<GoodDto>> GetGoodsWithPaging(PagingDto pagingParams)
    {
        throw new NotImplementedException();
    }
}