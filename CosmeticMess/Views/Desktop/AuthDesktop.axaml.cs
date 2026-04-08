using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class AuthDesktop : Page
{
    public AuthDesktop()
    {
        InitializeComponent();
    }

    private async void Auth_OnClick(object? sender, RoutedEventArgs e)
    {
        if (await API.Instance.Auth(LoginBox.Text, PasswordBox.Text))
        {
            NavigationService.Navigate(new MainDesktop());
        }
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }
}