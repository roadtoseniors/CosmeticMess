using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class AccountDesktop : Page
{
    public User User { get; set; } = API.Instance.AuthUser;
    public ObservableCollection<Order> Orders { get; set; } = new();
    public ObservableCollection<Record> Records { get; set; } = new();
    public ObservableCollection<OrderStatus> OrderStatusList { get; } = new();
    public ObservableCollection<RecordStatus> RecordStatusList { get; } = new();
    public ObservableCollection<ServiceType> ServiceTypes { get; set; } = new();
        
    public AccountDesktop()
    {
        InitializeComponent();
        
        LoadOrder();
        LoadRecords();
        DataContext = this;
    }

    private void ToMain_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }

    private async void LoadOrder()
    {
        var orders = await API.Instance.GetOrders();
        orders.Where(o => o.UserId == API.Instance.AuthUser.Id).ToList().ForEach(o => Orders.Add(o));
        var orderStatuses = await API.Instance.GetOrderStatuses();
        
        if (Orders.Any() || OrderStatusList.Any()) return;
    }
    
    private async void LoadRecords()
    {
        var records = await API.Instance.GetRecords();
        records.Where(r => r.ClientId == API.Instance.AuthUser.Id).ToList().ForEach(r => Records.Add(r));
        var recordStatuses = await API.Instance.GetRecordStatuses();
        
        if(Records.Any() ||  RecordStatusList.Any()) return;  
    }
}