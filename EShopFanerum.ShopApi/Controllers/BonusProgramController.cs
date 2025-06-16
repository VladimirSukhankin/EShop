using EShopFanerum.Infrastructure;
using EShopFanerum.Infrastructure.Dto.Shop;
using EShopFanerum.Infrastructure.Services;

namespace EShopFanerum.ShopApi.Controllers;

public class BonusProgramController : GenericApiController<BonusProgramDto>
{
    public BonusProgramController(IGenericService<BonusProgramDto, long> service) : base(service)
    {
    }
}