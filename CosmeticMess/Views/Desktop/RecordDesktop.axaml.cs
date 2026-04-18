using System.Collections.Generic;
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

    private List<Record> allRecords = new();
    
    public RecordDesktop()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private void Filtre_OnClick(object? sender, RoutedEventArgs e)
    {
        var dateFrom = DateFrom.SelectedDate?.Date;
        var dateTo = DateTo.SelectedDate?.Date;

        var filtre = allRecords
            .Where(r => !dateFrom.HasValue || r.Date.Date >= dateFrom.Value)
            .Where(r => !dateTo.HasValue   || r.Date.Date <= dateTo.Value);

        Records.Clear();
        filtre.ToList().ForEach(r => Records.Add(r));
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }

    private async void Load()
    {
        var records = await API.Instance.GetRecords();
        allRecords = records.Where(r => r.ClientId == null).ToList();
        
        Records.Clear();
        allRecords.ForEach(r => Records.Add(r));
        var recordStatuses = await API.Instance.GetRecordStatuses();
        
    }

    private void SelectedRecord_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is Record record)
        {
            NavigationService.Navigate(new SelectedRecord(record));
        }
    }

    private void Reset_OnClick(object? sender, RoutedEventArgs e)
    {
        DateFrom.SelectedDate = null;
        DateTo.SelectedDate = null;
        
        Records.Clear();
        allRecords.ForEach(r => Records.Add(r));
    }
}