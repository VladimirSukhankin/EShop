
using EShopFanerum.Service.BusService.Model;

namespace EShopFanerum.Infrastructure.Services;

public interface ISupportService
{
    Task SendMessageToSupport(SupportMessageDto supportMessageDto, CancellationToken cancellationToken);
}