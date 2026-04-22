using System;
using System.Collections.ObjectModel;
using Avalonia;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class MasterDesktop : Page
{
    public ObservableCollection<Record> Records { get; set; } = new();
    public ObservableCollection<MasterService> MyServices { get; set; } = new();
    public ObservableCollection<ServiceType> AvailableServices { get; set; } = new();

    public User mAster { get; set; } = API.Instance.AuthUser;

    public MasterDesktop()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private async void Load()
    {
        var records = await API.Instance.GetRecords();
        records.Where(r => r.MasterId == mAster.Id).ToList().ForEach(r => Records.Add(r));
        
        var masterServices = await API.Instance.GetMasterServices();
        masterServices.Where(ms => ms.UserId == mAster.Id).ToList().ForEach(ms => MyServices.Add(ms));
        
        var allServices = await API.Instance.GetServiceTypes();
        var myServiceTypeIds = MyServices.Select(ms => ms.ServiceTypeId).ToHashSet();
        allServices.Where(s => !myServiceTypeIds.Contains(s.Id)).ToList().ForEach(s => AvailableServices.Add(s));
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }

    private async void RemoveService_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not MasterService ms) return;

        await API.Instance.DeleteMasterService(ms.Id);
        MyServices.Remove(ms);
        
        AvailableServices.Add(ms.ServiceType);
    }

    private async void AddService_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ServiceComboBox.SelectedItem is not ServiceType serviceType) return;

        var ms = new MasterService
        {
            UserId = mAster.Id,
            ServiceTypeId = serviceType.Id,
            ServiceType = serviceType
        };

        var result = await API.Instance.PostMasterService(ms);
        if (result is null) return;

        MyServices.Add(result);
        AvailableServices.Remove(serviceType);
        ServiceComboBox.SelectedItem = null;
    }

    private void Record_OnClick(object? sender, PointerPressedEventArgs e)
    {
        if ((sender as Border)?.DataContext is Record record)
        {
            NavigationService.Navigate(new DetailRecordDesktop(record));
        }
    }
}