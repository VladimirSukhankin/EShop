using System.Threading.Tasks;
using EShopFanerum.Avalonia.ManagerApp.Models;

namespace EShopFanerum.Avalonia.ManagerApp.Serivces;

public interface IOrderService
{
   Task<OrderModel> GetOrders();
}