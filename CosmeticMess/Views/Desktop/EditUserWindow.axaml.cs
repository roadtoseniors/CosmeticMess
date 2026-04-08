using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using System.Collections.Generic;

namespace CosmeticMess.Views.Desktop;

public partial class EditUserWindow : Window
{
    private User _user;
    public EditUserWindow(User user)
    {
        InitializeComponent();
        _user = user;
        
        NameBox.Text = user.Name;
        LastNameBox.Text = user.LastName;
        PhoneBox.Text = user.Phone;
        LoginBox.Text = user.Login;
        AgeBox.Text = user.Age?.ToString();
        FrozenBox.IsChecked = user.IsFrozen;

        foreach (ComboBoxItem item in RoleBox.Items)
        {
            if (item.Tag is string tag && tag == user.Role.ToString())
            {
                RoleBox.SelectedItem = item;
                break;
            }
        }
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void  Save_OnClick(object? sender, RoutedEventArgs e)
    {
        _user.Name = NameBox.Text ?? _user.Name;
        _user.LastName = LastNameBox.Text ?? _user.LastName;
        _user.Phone = PhoneBox.Text;
        _user.Login = LoginBox.Text;
        _user.Age = int.TryParse(AgeBox.Text, out int age) ? age : _user.Age;
        _user.IsFrozen = FrozenBox.IsChecked ?? false;

        if (RoleBox.SelectedItem is ComboBoxItem selectedItem &&
            int.TryParse(selectedItem.Tag?.ToString(), out var roleId))
        {
            _user.RoleId = roleId;
        }
        
        var result = await API.Instance.PutUsers(_user);
        if (result != null)
        {
            Close();
        }
    }
}