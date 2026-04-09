using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaMessageBox;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class RegistrationDesktop : Page
{
    public RegistrationDesktop()
    {
        InitializeComponent();
        MessageBoxLocalization.Language = MessageBoxLanguage.Russian;
    }

    private async void Registration_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(LastName.Text) || string.IsNullOrWhiteSpace(Login.Text)
            || string.IsNullOrWhiteSpace(Password.Text) || string.IsNullOrWhiteSpace(Number.Text) ||
            string.IsNullOrWhiteSpace(Age.Text))
        {
            MessageBox.Show("Не все поля заполнены");
        }
        
        var user = new User
        {
            Name = Name.Text.Trim(),
            LastName = LastName.Text.Trim(),
            Phone = Number.Text.Trim(),
            Age = int.TryParse(Age.Text.Trim(), out var age) ? age : null,
            Login = Login.Text.Trim(),
            Password = Password.Text.Trim(),
            RoleId = 1,
            IsFrozen = false
        };
        
        var result = await API.Instance.Register(user);

        if (result == null)
        {
            MessageBox.Show("Проблема с регистрацией");
        }

        NavigationService.Navigate(new AccountDesktop());
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }
}