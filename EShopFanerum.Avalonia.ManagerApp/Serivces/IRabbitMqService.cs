using RabbitMQ.Client;

namespace EShopFanerum.Avalonia.ManagerApp.Serivces;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}