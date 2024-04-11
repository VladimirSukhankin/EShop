using System.Text;
using EShopFanerum.Core.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EShopFanerum.Core.Helpers;

public class RabbitMqSenderService : IRabbitMqSenderService
{
    public void SendMessage(object obj)
    {
        var message = JsonConvert.SerializeObject(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        // Не забудьте вынести значения "localhost" и "MyQueue"
        // в файл конфигурации
        var factory = new ConnectionFactory()
        {
            UserName = "rmuser",
            Password = "rmpassword",
            HostName = "localhost"
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                routingKey: "Orders",
                basicProperties: null,
                body: body);
        }
    }
}