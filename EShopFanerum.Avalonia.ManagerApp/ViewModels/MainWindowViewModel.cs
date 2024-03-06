using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using EShopFanerum.Avalonia.ManagerApp.Common.Helpers;
using EShopFanerum.Avalonia.ManagerApp.Models;
using EShopFanerum.Core.RabbitMQ.Dto;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
using ReactiveUI;
using Splat;

namespace EShopFanerum.Avalonia.ManagerApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IMessageSender _sender;
    public INotifyTaskCompletion InitializationNotifier { get; private set; }
    
    public ObservableCollection<OrderModel> Orders { get; }
    
    public MainWindowViewModel()
    {
        InitializationNotifier = NotifyTaskCompletion.Create(InitializeAsync());
        
        var serviceProvider = (IServiceProvider)Locator.Current.GetService(typeof(IServiceProvider))!;
        _sender = serviceProvider.GetRequiredService<IMessageSender>();
    }
    
    private async Task InitializeAsync()
    {
        await Task.Delay(2000);
    }
    
    
    public string Greeting => "Welcome to Avalonia!";
}