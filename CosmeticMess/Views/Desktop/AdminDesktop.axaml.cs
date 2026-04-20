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
    private ObservableCollection<User> Users = new();
    public AdminDesktop()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private async void Load()
    {
        var users = await API.Instance.GetUsers();
        users.ForEach(u => Users.Add(u));
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }
    
    private async void Frozen_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not User user)
        {
            return;
        }
        user.IsFrozen = !user.IsFrozen;
        await API.Instance.PutUsers(user);
        
        var id = Users.IndexOf(user);
        Users.Remove(user);
        Users.Insert(id, user);
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenEditWindow(new User(), true);
    }

    private void Edit_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not User user)
        {
            return;
        }
        OpenEditWindow(user, false);
    }
    
    private void OpenEditWindow(User user, bool isNew)
    {
        var window = new EditUserWindow(user, isNew);
        window.Closed += async (_, _) => await Reload();
        window.ShowDialog(null);
    }

    private async Task Reload()
    {
        Users.Clear();
        var users = await API.Instance.GetUsers();
        users.ForEach(u => Users.Add(u));
    }
}