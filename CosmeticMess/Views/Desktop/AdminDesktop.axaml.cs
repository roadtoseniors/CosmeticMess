using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class AdminDesktop : Page
{
    private ObservableCollection<User> _users = new();
    private List<Role> _roles = new();
    public AdminDesktop()
    {
        InitializeComponent();
        _ = Load();
    }

    private async Task Load()
    {
        var users = await API.Instance.GetUsers();
        var roles = await API.Instance.GetRoles();

        if (users == null || roles == null) return;

        _roles = roles;
        _users.Clear();
        
        foreach (var u in users)
        {
            u.Role = roles.FirstOrDefault(r => r.Id == u.RoleId);
            _users.Add(u);
        }

        TableUser.ItemsSource = _users;
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is User user)
        {
            var success = await API.Instance.DeleteUsers(user);
            if (!success)
            {
                _users.Remove(user);
            }
        }
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }

    private async void Update_OnClick(object? sender, RoutedEventArgs e)
    {
        await Load();
    }

    private async void Edit_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is User user)
        {
            var parentWindow = TopLevel.GetTopLevel(this) as Window;
            if (parentWindow == null) return;

            var dialog = new EditUserWindow(user);
            await dialog.ShowDialog(parentWindow);
            await Load();
        }
    }
}