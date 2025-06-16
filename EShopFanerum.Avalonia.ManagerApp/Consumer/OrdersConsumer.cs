using System;
using System.Threading.Tasks;
using Avalonia.Notification;
using Avalonia.Threading;
using EShopFanerum.Avalonia.ManagerApp.Domain;
using EShopFanerum.Avalonia.ManagerApp.Persistence;
using EShopFanerum.Avalonia.ManagerApp.ViewModels;
using EShopFanerum.Domain.Enums;
using EShopFanerum.Service.BusService.Model;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Avalonia.ManagerApp.Consumer;

public class OrderConsumer : IConsumer<OrderDto>
{
    private readonly ManagerDbContext _managerDbContext;
    private readonly MainWindowViewModel _vm;

    public OrderConsumer(ManagerDbContext managerDbContext, MainWindowViewModel vm)
    {
        _managerDbContext = managerDbContext;
        _vm = vm;
    }

    public async Task Consume(ConsumeContext<OrderDto> context)
    {
        var order = context.Message;
        
        if (!await _managerDbContext.Orders.AsNoTracking()
                .AnyAsync(x => x.Guid == order.Guid))
        {
            await Dispatcher.UIThread.InvokeAsync(() => 
            {
                _vm.Manager.CreateMessage()
                    .Accent("#1751C3")
                    .Animates(true)
                    .Background("#333")
                    .HasBadge("Info")
                    .HasMessage($"Необходимо обработать заказ №{order.Guid}.")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(10))
                    .Queue();
            });

            _managerDbContext.Orders.Add(new Order
            {
                CreatedDateTime = DateTime.UtcNow,
                Guid = order.Guid,
                Price = order.Price,
                GoodIds = order.GoodsIds.ToArray(),
                StateOrder = StateOrder.New
            });

            await _managerDbContext.SaveChangesAsync();
        }
    }
}