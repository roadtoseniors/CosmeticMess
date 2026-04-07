using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class AdminDesktop : Page
{
    public AdminDesktop()
    {
        InitializeComponent();
        Load();
    }

    private async void Load()
    {
        var users = await API.Instance.GetUsers();
        var roles = await API.Instance.GetRoles();

        if (users == null || roles == null) return;

        var result = new ObservableCollection<User>();
        foreach (var u in users)
        {
            u.Role = roles.FirstOrDefault(r => r.Id == u.RoleId);
            result.Add(u);
        }

        TableUser.ItemsSource = result;
    }
}