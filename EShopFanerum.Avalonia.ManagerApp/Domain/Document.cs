namespace EShopFanerum.Avalonia.ManagerApp.Domain;

public class Document
{
    public long Id { get; set; }

    public required string NameDocument { get; set; }

    public string? Description { get; set; }
}