namespace EShopFanerum.Core.RabbitMQ.Dto;

public class SupportMessageDto
{
    public long UserId { get; set; }

    public required string DescriptionError { get; set; }

    public required string Path { get; set; }
}