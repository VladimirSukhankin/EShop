namespace EShopFanerum.Domain.Entites.Manage;

public class Document
{
    public long Id { get; set; }

    public required string NameDocument { get; set; }

    public string? Description { get; set; }
}