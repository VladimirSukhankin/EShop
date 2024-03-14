using EShopFanerum.Infrastructure;
using EShopFanerum.Infrastructure.Dto.Stock;
using EShopFanerum.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.StockApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SupplierController : GenericApiController<SupplierDto>
{
    public SupplierController(IGenericService<SupplierDto, long> service) : base(service)
    {
    }
}