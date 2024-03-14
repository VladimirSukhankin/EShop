using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace EShopFanerum.Avalonia.ManagerApp.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}