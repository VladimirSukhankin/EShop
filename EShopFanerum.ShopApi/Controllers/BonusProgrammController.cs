using EShopFanerum.Infrastructure;
using EShopFanerum.Infrastructure.Dto.Shop;
using EShopFanerum.Infrastructure.Services;

namespace EShopFanerum.ShopApi.Controllers;

public class BonusProgrammController : GenericApiController<BonusProgramDto>
{
    public BonusProgrammController(IGenericService<BonusProgramDto, long> service) : base(service)
    {
    }
}