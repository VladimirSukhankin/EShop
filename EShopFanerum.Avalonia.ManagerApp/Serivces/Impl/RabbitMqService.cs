using RabbitMQ.Client;

namespace EShopFanerum.Avalonia.ManagerApp.Serivces.Impl;

public class RabbitMqService : IRabbitMqService
{
    public IConnection CreateChannel()
    {
        ConnectionFactory connection = new ConnectionFactory()
        {
            UserName = "rmuser",
            Password = "rmpassword",
            HostName = "localhost"
        };
        connection.DispatchConsumersAsync = true;
        var channel = connection.CreateConnection();
        return channel;
    }
}