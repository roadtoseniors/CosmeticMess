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

public partial class MainDesktop : Page
{
    public ObservableCollection<User> Users { get; } = new();
    public ObservableCollection<ServiceType> ServiceTypes { get; } = new();
    public ObservableCollection<MasterService> MasterServices { get; } = new();
    public MainDesktop()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private void Authorization_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new AuthDesktop());
    }

    private void Registration_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new RegistrationDesktop());
    }
    
    private async void Load()
    {
        var users = await API.Instance.GetUsers();
        var serviceTypes = await API.Instance.GetServiceTypes();
        var masterServices = await API.Instance.GetMasterServices();

        if (users == null || serviceTypes == null || masterServices == null) return;
        users = users.Where(u => u.RoleId == 2).ToList();
        foreach (var u in users)
        {
            Users.Add(u);
        }
    }

    private void AdminPanel_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new AdminDesktop());
    }

    private void Account_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new AccountDesktop());
    }

    private void Record_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new RecordDesktop());
    }
}