using System;

namespace EShopFanerum.Avalonia.ManagerApp.Models;

public class OrderModel
{
    public string NameGoods { get; set; }

    public string StateOrder { get; set; }

    public decimal Price { get; set; }
    
    public DateTime CreatedDateTime { get; set; }
}