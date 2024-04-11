using Avalonia.Controls;
using EShopFanerum.Avalonia.ManagerApp.ViewModels;

namespace EShopFanerum.Avalonia.ManagerApp.Views;

public partial class MainWindow : Window
{
    
    private MainWindowViewModel _vm;
    public MainWindow(MainWindowViewModel vm)
    {
        _vm = vm;
        DataContext = _vm;
        InitializeComponent();
    }
}