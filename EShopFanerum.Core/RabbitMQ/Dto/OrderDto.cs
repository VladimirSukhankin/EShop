namespace EShopFanerum.Core.RabbitMQ.Dto;

public class OrderDto
{
    public Guid Guid { get; set; }

    public List<long> GoodsIds { get; set; } = new List<long>();
    public decimal Price { get; set; }
    public int Count { get; set; }
}