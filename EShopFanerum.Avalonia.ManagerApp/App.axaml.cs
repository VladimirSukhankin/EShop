using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EShopFanerum.Avalonia.ManagerApp.Consumer;
using EShopFanerum.Avalonia.ManagerApp.Extensions;
using EShopFanerum.Avalonia.ManagerApp.ViewModels;
using EShopFanerum.Avalonia.ManagerApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EShopFanerum.Avalonia.ManagerApp;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    
    
    public override void OnFrameworkInitializationCompleted()
    {
        // If you use CommunityToolkit, line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        // Register all the services needed for the application to run
        var collection = new ServiceCollection();
        collection.AddServices();
        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();
        
        
        BackgroundService backgroundService = new OrderHostedService(services.GetRequiredService<IConsumerService>());
        
        backgroundService.StartAsync(CancellationToken.None);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(services.GetRequiredService<MainWindowViewModel>());
        }
        
        base.OnFrameworkInitializationCompleted();
    }
}