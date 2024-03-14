using System.ComponentModel;

namespace EShopFanerum.Domain.Enums;

public enum StateOrder
{
    [Description("Новый")]
    New,
    
    [Description("В обработке")]
    InProcess,
    
    [Description("Завершён")]
    Complete,
    
    [Description("Отменён")]
    Cancell,
}