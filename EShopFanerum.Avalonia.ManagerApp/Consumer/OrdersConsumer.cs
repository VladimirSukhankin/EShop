using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Notification;
using Avalonia.Threading;
using EShopFanerum.Avalonia.ManagerApp.Domain;
using EShopFanerum.Avalonia.ManagerApp.Persistence;
using EShopFanerum.Avalonia.ManagerApp.Serivces;
using EShopFanerum.Avalonia.ManagerApp.ViewModels;
using EShopFanerum.Core.RabbitMQ.Dto;
using EShopFanerum.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EShopFanerum.Avalonia.ManagerApp.Consumer;

public class OrdersConsumer : IConsumerService, IDisposable
{
    private const string QueueName = "Orders";

    private ManagerDbContext _managerDbContext;
    private MainWindowViewModel _vm;
    private readonly IModel _model;
    private readonly IConnection _connection;

    public OrdersConsumer(IRabbitMqService rabbitMqService, ManagerDbContext managerDbContext, MainWindowViewModel vm)
    {
        _managerDbContext = managerDbContext;
        _vm = vm;
        _connection = rabbitMqService.CreateChannel();
        _model = _connection.CreateModel();
        _model.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);
    }

    public async Task ReadMessages()
    {
        var consumer = new AsyncEventingBasicConsumer(_model);
        consumer.Received += async (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            var text = System.Text.Encoding.UTF8.GetString(body);
            var result = JObject.Parse(text);
            
            var order = result.ToObject<OrderDto>();
            
            if (!_managerDbContext.Orders.AsNoTracking().Any(x => x.Guid == order!.Guid))
            {
                Dispatcher.UIThread.Invoke(()=> _vm.Manager.CreateMessage()
                    .Accent("#1751C3")
                    .Animates(true)
                    .Background("#333")
                    .HasBadge("Info")
                    .HasMessage(
                        $"Необходимо обработать заказ №{order.Guid}.")
                    .Dismiss().WithDelay(TimeSpan.FromSeconds(10))
                    .Queue());
                
                
                _managerDbContext.Orders.Add(new Order()
                {
                    CreatedDateTime = DateTime.UtcNow,
                    Guid = order.Guid,
                    Price = order.Price,
                    GoodIds = order.GoodsIds.ToArray(),
                    StateOrder = StateOrder.New
                });
                await _managerDbContext.SaveChangesAsync(CancellationToken.None);
          
                Console.WriteLine(order.Guid);
            }

            _model.BasicAck(ea.DeliveryTag, false);
        };
        _model.BasicConsume(QueueName, false, consumer);
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_model.IsOpen)
            _model.Close();
        if (_connection.IsOpen)
            _connection.Close();
    }
}