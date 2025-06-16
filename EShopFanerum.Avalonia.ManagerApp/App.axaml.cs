using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EShopFanerum.Avalonia.ManagerApp.Extensions;
using EShopFanerum.Avalonia.ManagerApp.ViewModels;
using EShopFanerum.Avalonia.ManagerApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EShopFanerum.Avalonia.ManagerApp;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Remove Avalonia data validation if using CommunityToolkit
        BindingPlugins.DataValidators.RemoveAt(0);

        // Configure services
        var services = new ServiceCollection();
        services.AddServices();
        
        var serviceProvider = services.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(
                serviceProvider.GetRequiredService<MainWindowViewModel>());
        }

        base.OnFrameworkInitializationCompleted();
    }
}