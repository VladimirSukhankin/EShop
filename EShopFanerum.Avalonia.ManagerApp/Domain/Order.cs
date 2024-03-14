using System;
using System.ComponentModel.DataAnnotations;
using EShopFanerum.Domain.Enums;

namespace EShopFanerum.Avalonia.ManagerApp.Domain;

//Заказы
public class Order
{
    [Key]
    public Guid Guid { get; set; }
    
    public long[] GoodIds { get; set; }

    public decimal Price { get; set; }

    public StateOrder StateOrder { get; set; }

    public DateTime CreatedDateTime { get; set; }
}