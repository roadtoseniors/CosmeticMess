using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class RecordDesktop : Page
{
    public ObservableCollection<User> Users { get; } = new();
    public ObservableCollection<Record> Records { get; } = new();
    public ObservableCollection<ServiceType> ServiceTypes { get; } = new();
    public ObservableCollection<PaymentType> PaymentTypes { get; } = new();
    public ObservableCollection<RecordStatus> RecordStatus { get; } = new();
    public RecordDesktop()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private void Filtre_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }

    private async void Load()
    {
        var users = await API.Instance.GetUsers();
        var records = await API.Instance.GetRecords();
        var serviceTypes = await API.Instance.GetServiceTypes();
        var paymentTypes = await API.Instance.GetPaymentTypes();
        var recordStatuses = await API.Instance.GetRecordStatuses();
    }
}