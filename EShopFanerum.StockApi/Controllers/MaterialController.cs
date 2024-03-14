using EShopFanerum.Infrastructure;
using EShopFanerum.Infrastructure.Dto.Stock;
using EShopFanerum.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.StockApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MaterialController : GenericApiController<MaterialDto>
{
    public MaterialController(IGenericService<MaterialDto, long> service) : base(service)
    {
    }
}