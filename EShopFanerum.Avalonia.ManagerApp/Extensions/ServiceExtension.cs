using Avalonia.Notification;
using EShopFanerum.Avalonia.ManagerApp.Consumer;
using EShopFanerum.Avalonia.ManagerApp.Persistence;
using EShopFanerum.Avalonia.ManagerApp.ViewModels;
using MassTransit;
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
        
        services.AddMassTransitConfiguration();
    }

    public static void AddCommonService(this IServiceCollection services)
    {
        services.AddHostedService<OrderHostedService>();
    }

    private static void AddMassTransitConfiguration(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("rmuser");
                    h.Password("rmpassword");
                });
                cfg.ReceiveEndpoint("Orders", e =>
                {
                    e.ConfigureConsumer<OrderConsumer>(context);
                    e.PrefetchCount = 10;
                    e.ConcurrentMessageLimit = 5;
                });
            });
        });
    }
}