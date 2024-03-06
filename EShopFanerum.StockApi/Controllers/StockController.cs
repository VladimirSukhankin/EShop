using EShopFanerum.Infrastructure.Requests;
using EShopFanerum.Infrastructure.Requests.Good;
using EShopFanerum.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.StockApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly IGoodService _goodService;

    public StockController(IGoodService goodService)
    {
        _goodService = goodService;
    }
    
    [HttpPost("good")]
    public async Task AddGoodAsync(AddGoodRequest addGoodRequest, CancellationToken cancellationToken)
    {
        await _goodService.AddGoodAsync(addGoodRequest, cancellationToken);
    }
    
    [HttpPut("good}")]
    public async Task UpdateGoodAsync(UpdateGoodRequest updateGoodRequest, CancellationToken cancellationToken)
    {
        await _goodService.UpdateGoodAsync(updateGoodRequest, cancellationToken);
    }
  
    [HttpDelete("good/{idGood}")]
    public async Task DeleteGoodAsync(long idGood, CancellationToken cancellationToken)
    {
        await _goodService.DeleteGoodAsync(idGood, cancellationToken);
    }
    
    //Поставщик
    //Материалы
    // Бонусные программы
}