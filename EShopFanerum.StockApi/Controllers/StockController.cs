using EShopFanerum.Infrastructure;
using EShopFanerum.Infrastructure.Dto.Shop;
using EShopFanerum.Infrastructure.Requests.Good;
using EShopFanerum.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.StockApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : GenericApiController<GoodDto>
{
    private readonly IGoodService _goodService;
    
    public StockController(IGenericService<GoodDto, long> service, IGoodService goodService) : base(service)
    {
        _goodService = goodService;
    }
    
    [HttpPost("goodByIds")]
    public async Task<ICollection<GoodDto>> GetGoodsByIds(GetGoodByIdsRequest getGoodsRequest, CancellationToken cancellationToken)
    {
        return await _goodService.GetGoodsByIdsAsync(getGoodsRequest.GoodIds, cancellationToken);
    }
    
}