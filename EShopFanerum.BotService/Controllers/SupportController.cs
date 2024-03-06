using EShopFanerum.Core.RabbitMQ.Dto;
using EShopFanerum.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.BotService.Controllers;

[ApiController]
[Route("[controller]")]
public class SupportController : ControllerBase
{
    private readonly ISupportService _supportService;
    
    public SupportController(ISupportService supportService)
    {
        _supportService = supportService;
    }
    [HttpPut("support")]
    public async Task SendMessage(SupportMessageDto message, CancellationToken cancellationToken)
    {
        await _supportService.SendMessageToSupport(message, cancellationToken);
    }
}