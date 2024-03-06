using EShopFanerum.Core.RabbitMQ.Dto;

namespace EShopFanerum.Infrastructure.Services;

public interface ISupportService
{
    Task SendMessageToSupport(SupportMessageDto supportMessageDto, CancellationToken cancellationToken);
}