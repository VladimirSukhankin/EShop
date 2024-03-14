namespace EShopFanerum.Avalonia.ManagerApp.Models;

public class GoodModel
{
    public long Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? CategoryName { get; set; }

    public decimal Price { get; set; }
}