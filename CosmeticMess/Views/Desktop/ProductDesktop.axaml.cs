using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class ProductDesktop : Page
{
    public ObservableCollection<Product> FiltreProducts { get; set; } = new();
    public ObservableCollection<ProductType> ProductTypes { get; set; } = new();
    public ObservableCollection<Manufacturer> Manufacturers { get; set; } = new();
    
    private List<Product> allProducts = new();
    public ProductDesktop()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private async void Load()
    {
        var products = await API.Instance.GetProducts();
        var types = await API.Instance.GetProductTypes();
        var manufacturers = await API.Instance.GetManufacturers();

        allProducts = products;
        allProducts.ForEach(p => FiltreProducts.Add(p));
        types.ForEach(t => ProductTypes.Add(t));
        manufacturers.ForEach(m => Manufacturers.Add(m));
    }

    private void ApplyFiltre()
    {
        var search = SearchBox.Text?.ToLower() ?? "";
        var type = TypeFiltre.SelectedItem as ProductType;
        var manufacturer = TypeManufacturer.SelectedItem as Manufacturer;

        var result = allProducts.Where(p => string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search))
            .Where(p => type == null || p.ProductTypeId == type.Id)
            .Where(p => manufacturer == null || p.ManufacturerId == manufacturer.Id);

        result = SortBox.SelectedIndex switch
        {
            0 => result.OrderByDescending(p => p.Rating ?? 0),
            1 => result.OrderBy(p => p.Rating ?? 0),
            _ => result
        };
        
        FiltreProducts.Clear();
        result.ToList().ForEach(p => FiltreProducts.Add(p));
    }

    private void Search_OnChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFiltre();
    }

    private void Filter_OnChanged(object? sender, SelectionChangedEventArgs e)
    {
        ApplyFiltre();
    }

    private void Reset_OnClick(object? sender, RoutedEventArgs e)
    {
        SearchBox.Text = "";
        TypeFiltre.SelectedItem = null;
        TypeManufacturer.SelectedItem = null;
        SortBox.SelectedItem = null;
        FiltreProducts.Clear();
        allProducts.ForEach(p => FiltreProducts.Add(p));
    }

    private void Basket_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new BasketDesktop());
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }

    private void Product_OnClick(object? sender, PointerPressedEventArgs e)
    {
        if ((sender as Border)?.DataContext is Product product)
        {
            //NavigationService.Navigate(new ProductDetailDesktop(product));
        }
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