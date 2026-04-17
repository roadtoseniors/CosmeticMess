using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class RecordDesktop : Page
{
    public User User { get; set; } = API.Instance.AuthUser;
    public ObservableCollection<Record> Records { get; set; } = new();
    public ObservableCollection<RecordStatus> RecordStatusList { get; set; } = new();
    public ObservableCollection<ServiceType> ServiceTypes { get; set; } = new();
    public RecordDesktop()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private void Filtre_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }

    private async void Load()
    {
        var records = await API.Instance.GetRecords();
        records.Where(r => r.ClientId == null).ToList().ForEach(r => Records.Add(r));
        var recordStatuses = await API.Instance.GetRecordStatuses();
        
        if(Records.Any() || RecordStatusList.Any()) return;
    }

    private void SelectedRecord_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new SelectedRecord());
    }
}