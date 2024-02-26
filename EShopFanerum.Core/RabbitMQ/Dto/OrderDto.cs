namespace EShopFanerum.Core.RabbitMQ.Dto;

public class OrderDto
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
}