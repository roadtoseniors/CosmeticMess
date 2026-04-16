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
    public User User { get; set; } = new User();
    public ObservableCollection<Order> Orders { get; } = new();
    public ObservableCollection<Record> Records { get; } = new();
    public ObservableCollection<OrderStatus> OrderStatusList { get; } = new();
    public ObservableCollection<RecordStatus> RecordStatusList { get; } = new();
    public ObservableCollection<ServiceType> ServiceTypes { get; } = new();
        
    public AccountDesktop()
    {
        InitializeComponent();
        DataContext = this;
        LoadOrder();
        LoadRecords();
    }

    private void ToMain_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }

    private async void LoadOrder()
    {
        var orders = await API.Instance.GetOrders();
        var orderStatuses = await API.Instance.GetOrderStatuses();
        
        if (orders == null || orderStatuses == null) return;
        orders.Where(o => o.UserId == API.Instance.AuthUser.Id).ToList();
        

    }
    
    private async void LoadRecords()
    {
        var recordStatuses = await API.Instance.GetRecordStatuses();
        var records = await API.Instance.GetRecords();
        
        if(records == null ||  recordStatuses == null) return;  
        
        records.Where(r => r.ClientId == API.Instance.AuthUser.Id).ToList();
    }
}