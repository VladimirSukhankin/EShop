using System;
using System.Collections.Generic;

namespace EShopFanerum.Avalonia.ManagerApp.Models;

public class OrderModel
{
    public Guid Guid { get; set; }

    public List<long> GoodIds { get; set; }
    public string NameGoods { get; set; }

    public string StateOrder { get; set; }

    public decimal Price { get; set; }
    
    public DateTime CreatedDateTime { get; set; }

    public OrderModel(Guid guid, List<long> goodIds, string stateOrder, decimal price, DateTime createdDateTime, string nameGoods)
    {
        Guid = guid;
        GoodIds = goodIds;
        StateOrder = stateOrder;
        Price = price;
        CreatedDateTime = createdDateTime;
        NameGoods = nameGoods;
    }
    
}