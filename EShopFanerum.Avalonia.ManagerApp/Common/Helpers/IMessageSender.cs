using System.Threading.Tasks;

namespace EShopFanerum.Avalonia.ManagerApp.Common.Helpers;

public interface IMessageSender
{
    Task SendMessage<T>(T message);
}