using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;
namespace CosmeticMess.Views.Desktop;

public partial class BasketDesktop : Page
{
    public ObservableCollection<BasketItem> BasketItems { get; set; } = new();
    private Basket? _basket;

    public BasketDesktop()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private async void Load()
    {
        var user = API.Instance.AuthUser;
        if (user == null)
        {
            return;
        }

        _basket = await API.Instance.GetBasketByUser(user.Id);
        if (_basket == null)
        {
            _basket = await API.Instance.PostBasket(new Basket
            {
                UserId = user.Id,
                User = user
            });
        }

        if (_basket == null)
        {
            return;
        }
        
        var allItems = await API.Instance.GetBasketItems();
        allItems.Where(a => a.BasketId == _basket.Id).ToList().ForEach(a => BasketItems.Add(a));

        UpdateTotal();
    }

    private void UpdateTotal()
    {
        var total = BasketItems.Sum(i => i.Product.Price * i.Quantity);
        TotalText.Text = $"{total:# ##0.00} ₽";
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new ProductDesktop());
    }

    private async void Order_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!BasketItems.Any()) return;
        var parent = (Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime)?.MainWindow;
        var window = new OrderWindow(BasketItems.ToList(), _basket!);
        window.Closed += (_, _) =>
        {
            BasketItems.Clear();
            UpdateTotal();
        };
        window.ShowDialog(parent);
    }

    private async void Minus_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not BasketItem item) return;
        if (item.Quantity <= 1) return;
        item.Quantity--;
        await API.Instance.PutBasketItem(item);
        Refresh(item);
    }

    private async void Plus_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not BasketItem item) return;
        item.Quantity++;
        await API.Instance.PutBasketItem(item);
        Refresh(item);
    }
    
    private void Refresh(BasketItem item)
    {
        var idx = BasketItems.IndexOf(item);
        BasketItems.Remove(item);
        BasketItems.Insert(idx, item);
        UpdateTotal();
    }

    private async void Delete_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not BasketItem item) return;
        await API.Instance.DeleteBasketItem(item.Id);
        BasketItems.Remove(item);
        UpdateTotal();
    }
}