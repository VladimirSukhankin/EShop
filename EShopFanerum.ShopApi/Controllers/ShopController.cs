using EShopFanerum.Infrastructure.Dto;
using EShopFanerum.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.ShopApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ShopController : ControllerBase
{
    private readonly IGoodService _goodService;
    
    public ShopController(IGoodService goodService)
    {
        _goodService = goodService;
    }
    [HttpGet("goods")]
    public async Task<ICollection<GoodDto>> GetGoods(PagingDto pagingParams, CancellationToken cancellationToken)
    {
        return await _goodService.GetGoodsWithPagingAsync(pagingParams, cancellationToken);
    }
}