using EShopFanerum.Core.RabbitMQ.Dto;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.BotService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IBus _bus;
    
    public OrdersController(IBus bus)
    {
        _bus = bus;
    }
    
    [HttpPost(Name = "create")]
    public async Task<IActionResult> CreateOrder(OrderDto order, CancellationToken cancellationToken)
    {
        await _bus.Publish(order, cancellationToken);
        
        return Ok();
    }
}