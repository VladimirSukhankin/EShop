using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Notification;
using EShopFanerum.Avalonia.ManagerApp.Extensions;
using EShopFanerum.Avalonia.ManagerApp.Models;
using EShopFanerum.Avalonia.ManagerApp.Persistence;
using EShopFanerum.Domain.Enums;
using EShopFanerum.Infrastructure.Dto.Shop;
using EShopFanerum.Infrastructure.Dto.Stock;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Nito.AsyncEx;
using ReactiveUI.Fody.Helpers;

namespace EShopFanerum.Avalonia.ManagerApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ManagerDbContext _managerDbContext;


    private readonly IHttpClientFactory _httpClientFactory;
    public INotifyTaskCompletion InitializationNotifier { get; private set; }

    private bool _isNotFound;

    public bool IsNotFound
    {
        get => _isNotFound;
        set
        {
            _isNotFound = value;
            OnPropertyChanged();
        }
    }

    [Reactive] public ObservableCollection<OrderModel> Orders { get; }

    [Reactive] public ObservableCollection<MaterialModel> Materials { get; }

    [Reactive] public ObservableCollection<SupplierModel> Suppliers { get; }

    [Reactive] public INotificationMessageManager Manager { get; } = new NotificationMessageManager();

    public MainWindowViewModel(IHttpClientFactory httpClientFactory, ManagerDbContext dbContext)
    {
        Orders = new ObservableCollection<OrderModel>();
        Materials = new ObservableCollection<MaterialModel>();
        Suppliers = new ObservableCollection<SupplierModel>();

        _managerDbContext = dbContext;
        _httpClientFactory = httpClientFactory;

        InitializationNotifier = NotifyTaskCompletion.Create(InitializeAsync());
    }

    private async Task InitializeAsync()
    {
        await SetOrdersAsync();
        await SetMaterialsAsync();
        await SetSuppliersAsync();
    }

    private async Task SetOrdersAsync()
    {
        using var client = _httpClientFactory.CreateClient();

        var orders = await _managerDbContext.Orders.AsNoTracking().ToListAsync(CancellationToken.None);

        if (orders.Count == 0)
        {
            IsNotFound = true;
            return;
        }

        var orderModels = orders.Select(x => new OrderModel(
            x.Guid,
            x.GoodIds == null ? new List<long>() : x.GoodIds.ToList(),
            StateOrder.New.GetDescription(),
            x.Price,
            DateTime.UtcNow,
            x.Guid.ToString())
        ).ToList();

        foreach (var orderModel in orderModels)
        {
            var responseGoods = await client.PostAsJsonAsync("http://localhost:5188/Stock/goodByIds", new
            {
                goodIds = orderModel.GoodIds
            });
            var goods = JsonConvert.DeserializeObject<List<GoodDto>>(await responseGoods.Content.ReadAsStringAsync());
            if (goods != null)
            {
                orderModel.NameGoods = String.Join(", ", goods.Select(x => x.Name).ToArray());
            }
        }

        Orders.AddRange(orderModels);
    }

    private async Task SetMaterialsAsync()
    {
        using var client = _httpClientFactory.CreateClient();

        var responseMaterial = await client.GetAsync("http://localhost:5188/Material");
        var materialDto =
            JsonConvert.DeserializeObject<List<MaterialDto>>(await responseMaterial.Content.ReadAsStringAsync());
        var materialModels = materialDto!.Select(x => new MaterialModel()
        {
            Id = x.Id,
            Count = x.Count,
            Name = x.Name
        });

        Materials.AddRange(materialModels);
    }

    private async Task SetSuppliersAsync()
    {
        using var client = _httpClientFactory.CreateClient();

        var responseMaterial = await client.GetAsync("http://localhost:5188/Supplier");
        var supplierDto =
            JsonConvert.DeserializeObject<List<SupplierDto>>(await responseMaterial.Content.ReadAsStringAsync());
        var supplierModels = supplierDto!.Select(x => new SupplierModel()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Phone = x.Phone,
            Raiting = x.Raiting,
            IsActive = x.IsActive
        });

        Suppliers.AddRange(supplierModels);
    }
}