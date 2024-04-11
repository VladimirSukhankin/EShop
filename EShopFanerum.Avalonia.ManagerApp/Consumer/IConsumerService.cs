using System.Threading.Tasks;

namespace EShopFanerum.Avalonia.ManagerApp.Consumer;

public interface IConsumerService
{
    Task ReadMessages();
}