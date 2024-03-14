using EShopFanerum.Avalonia.ManagerApp.Common.Helpers;
using EShopFanerum.Avalonia.ManagerApp.Consumer;
using EShopFanerum.Avalonia.ManagerApp.Persistence;
using EShopFanerum.Avalonia.ManagerApp.ViewModels;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EShopFanerum.Avalonia.ManagerApp.Extensions;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host("localhost", "/", h =>
            {
                h.Username("rmuser");
                h.Password("rmpassword");
            });
            
            cfg.ReceiveEndpoint("Orders", ep =>
            {
                ep.Consumer<OrdersConsumer>();
            });
            cfg.ReceiveEndpoint("SupportMessage", ep =>
            {
                ep.Consumer<SupportMessageConsumer>();
            });
        });

        services.AddSingleton<IBus>(busControl);
        services.AddSingleton<IMessageSender, MessegeSender>();
        
        services.AddHttpClient();
        services.AddDbContext<ManagerDbContext>();
        services.AddTransient<MainWindowViewModel>();
        busControl.StartAsync();
    }

    public static void AddManageDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ManagerDbContext>(options =>
            options.UseNpgsql("Server=localhost;Port=5432;User ID=postgres;Database=manager;Password=1;TrustServerCertificate=True"));

    }
}