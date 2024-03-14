using EShopFanerum.Infrastructure.Dto;
using EShopFanerum.Infrastructure.Dto.Shop;

namespace EShopFanerum.Infrastructure.Services;

public interface IGoodService : IGenericService<GoodDto, long>
{
    public Task<List<GoodDto>> GetGoodsByIdsAsync(List<long> ids, CancellationToken cancellationToken);
}