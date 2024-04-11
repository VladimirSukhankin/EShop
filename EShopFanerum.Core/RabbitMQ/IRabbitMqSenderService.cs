namespace EShopFanerum.Core.RabbitMQ;

public interface IRabbitMqSenderService
{
    void SendMessage(object obj);
    void SendMessage(string message);
}