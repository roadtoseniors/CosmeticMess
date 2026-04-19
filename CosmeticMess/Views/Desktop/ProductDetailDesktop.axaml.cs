using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class ProductDetailDesktop : Page
{
    public Product Product { get; set; }
    public ProductDetailDesktop(Product product)
    {
        Product = product;
        InitializeComponent();
        DataContext = this;
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new ProductDesktop());
    }

    private void AddToBasket_OnClick(object? sender, RoutedEventArgs e)
    {
        if (API.Instance.AuthUser == null)
        {
            NavigationService.Navigate(new AuthDesktop());
            return;
        }
    }
}