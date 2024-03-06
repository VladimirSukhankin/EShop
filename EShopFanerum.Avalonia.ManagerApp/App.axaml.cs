using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EShopFanerum.Avalonia.ManagerApp.Common.Helpers;
using EShopFanerum.Avalonia.ManagerApp.Consumer;
using EShopFanerum.Avalonia.ManagerApp.Persistence;
using EShopFanerum.Avalonia.ManagerApp.ViewModels;
using EShopFanerum.Avalonia.ManagerApp.Views;
using EShopFanerum.Core.Helpers;
using EShopFanerum.Persistance.Exstension;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Splat;

namespace EShopFanerum.Avalonia.ManagerApp;

public partial class App : Application
{
    public override void Initialize()
    {
        var services = new ServiceCollection();
        
        var dbContext = new TestDbContext();
        
        dbContext.Database.EnsureCreated();
        
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

        /*services.AddDbContext<TestDbContext>(options =>
            options.UseNpgsql("Server=localhost;Port=5432;User ID=postgres;Database=TESTCONTEXT;Password=1;TrustServerCertificate=True"));
            */

        
        var serviceProvider = services.BuildServiceProvider();
        
        Locator.CurrentMutable.RegisterConstant(serviceProvider, typeof(IServiceProvider));

        busControl.StartAsync();
        
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}