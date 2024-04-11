using Avalonia.Notification;
using EShopFanerum.Avalonia.ManagerApp.Consumer;
using EShopFanerum.Avalonia.ManagerApp.Persistence;
using EShopFanerum.Avalonia.ManagerApp.Serivces;
using EShopFanerum.Avalonia.ManagerApp.Serivces.Impl;
using EShopFanerum.Avalonia.ManagerApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace EShopFanerum.Avalonia.ManagerApp.Extensions;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddCommonService();
        services.AddHttpClient();
        services.AddDbContext<ManagerDbContext>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<INotificationMessageManager, NotificationMessageManager>();
    }

    public static void AddCommonService(this IServiceCollection services)
    {
        services.AddHostedService<OrderHostedService>();
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
        services.AddTransient<IConsumerService, OrdersConsumer>();
    }
}