using EShopFanerum.Infrastructure.Dto;
using EShopFanerum.Infrastructure.Requests;
using EShopFanerum.Infrastructure.Requests.Good;

namespace EShopFanerum.Infrastructure.Services;

public interface IGoodService
{
    Task<ICollection<GoodDto>> GetGoodsWithPagingAsync(PagingDto pagingParams,  CancellationToken cancellationToken);

    Task AddGoodAsync(AddGoodRequest addGoodRequest, CancellationToken cancellationToken);

    Task UpdateGoodAsync(UpdateGoodRequest addGoodRequest, CancellationToken cancellationToken);
    
    Task DeleteGoodAsync(long idGood, CancellationToken cancellationToken);
}