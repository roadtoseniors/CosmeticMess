using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class ManagerDesktop : Page
{
    public ObservableCollection<Record>       Records       { get; set; } = new();
    public ObservableCollection<Order>        Orders        { get; set; } = new();
    public ObservableCollection<Product>      Products      { get; set; } = new();
    public ObservableCollection<Manufacturer> Manufacturers { get; set; } = new();
    public ObservableCollection<ProductType>  ProductTypes  { get; set; } = new();
    public ObservableCollection<ServiceType>  ServiceTypes  { get; set; } = new();

    private List<Record> _allRecords = new();

    public ManagerDesktop()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private async void Load()
    {
        var records      = await API.Instance.GetRecords();
        var orders       = await API.Instance.GetOrders();
        var products     = await API.Instance.GetProducts();
        var manufacturers = await API.Instance.GetManufacturers();
        var productTypes = await API.Instance.GetProductTypes();
        var serviceTypes = await API.Instance.GetServiceTypes();

        _allRecords = records;
        records.ForEach(r => Records.Add(r));
        orders.ForEach(o => Orders.Add(o));
        products.ForEach(p => Products.Add(p));
        manufacturers.ForEach(m => Manufacturers.Add(m));
        productTypes.ForEach(t => ProductTypes.Add(t));
        serviceTypes.ForEach(s => ServiceTypes.Add(s));
    }

    // ── Переключение вкладок ──────────────────────────────────────────────────

    private void Tab_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not string tag) return;

        RecordsTab.IsVisible       = tag == "Records";
        OrdersTab.IsVisible        = tag == "Orders";
        ProductsTab.IsVisible      = tag == "Products";
        ManufacturersTab.IsVisible = tag == "Manufacturers";
        ProductTypesTab.IsVisible  = tag == "ProductTypes";
        ServiceTypesTab.IsVisible  = tag == "ServiceTypes";
    }

    // ── Записи ────────────────────────────────────────────────────────────────

    private void ClientSearch_OnChanged(object? sender, TextChangedEventArgs e)
    {
        var q = ClientSearch.Text?.ToLower() ?? "";
        var filtered = string.IsNullOrWhiteSpace(q)
            ? _allRecords
            : _allRecords.Where(r =>
                (r.Client?.Name?.ToLower().Contains(q) ?? false) ||
                (r.Client?.LastName?.ToLower().Contains(q) ?? false) ||
                (r.Client?.Phone?.Contains(q) ?? false)).ToList();

        Records.Clear();
        filtered.ForEach(r => Records.Add(r));
    }

    private void AddRecord_OnClick(object? sender, RoutedEventArgs e) =>
        OpenWindow(new AddRecordWindow());

    private async void RescheduleRecord_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not Record record) return;
        OpenWindow(new RescheduleRecordWindow(record), async () => await ReloadRecords());
    }

    private async void CancelRecord_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not Record record) return;
        var statuses = await API.Instance.GetRecordStatuses();
        var cancelled = statuses.FirstOrDefault(s => s.Name == "Отменена") ?? statuses.Last();
        record.StatusId = cancelled.Id;
        record.Status = cancelled;
        await API.Instance.PutRecords(record);
        await ReloadRecords();
    }

    // ── Заказы ────────────────────────────────────────────────────────────────

    private async void CloseOrder_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not Order order) return;
        var statuses = await API.Instance.GetOrderStatuses();
        var closed = statuses.FirstOrDefault(s => s.Name == "Закрыт") ?? statuses.Last();
        order.StatusId = closed.Id;
        order.Status = closed;
        await API.Instance.PutOrders(order);
        await ReloadOrders();
    }

    // ── Товары ────────────────────────────────────────────────────────────────

    private void AddProduct_OnClick(object? sender, RoutedEventArgs e) =>
        OpenWindow(new ProductEditWindow(new Product(), isNew: true), async () => await ReloadProducts());

    private void EditProduct_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not Product product) return;
        OpenWindow(new ProductEditWindow(product, isNew: false), async () => await ReloadProducts());
    }

    private async void FreezeProduct_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not Product product) return;
        product.IsFrozen = !product.IsFrozen;
        await API.Instance.PutProducts(product);
        await ReloadProducts();
    }

    // ── Производители ─────────────────────────────────────────────────────────

    private void AddManufacturer_OnClick(object? sender, RoutedEventArgs e) =>
        OpenWindow(new ManufacturerEditWindow(new Manufacturer(), isNew: true), async () => await ReloadManufacturers());

    private void EditManufacturer_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not Manufacturer m) return;
        OpenWindow(new ManufacturerEditWindow(m, isNew: false), async () => await ReloadManufacturers());
    }

    // ── Типы товаров ──────────────────────────────────────────────────────────

    private void AddProductType_OnClick(object? sender, RoutedEventArgs e) =>
        OpenWindow(new SimpleEditWindow("Тип товара", "", async name =>
        {
            await API.Instance.PostProductType(new ProductType { Name = name });
            await ReloadProductTypes();
        }));

    private void EditProductType_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not ProductType t) return;
        OpenWindow(new SimpleEditWindow("Тип товара", t.Name, async name =>
        {
            t.Name = name;
            await API.Instance.PutProductType(t);
            await ReloadProductTypes();
        }));
    }

    // ── Услуги ────────────────────────────────────────────────────────────────

    private void AddServiceType_OnClick(object? sender, RoutedEventArgs e) =>
        OpenWindow(new ServiceTypeEditWindow(new ServiceType(), isNew: true), async () => await ReloadServiceTypes());

    private void EditServiceType_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not ServiceType s) return;
        OpenWindow(new ServiceTypeEditWindow(s, isNew: false), async () => await ReloadServiceTypes());
    }

    // ── Перезагрузка данных ───────────────────────────────────────────────────

    private async Task ReloadRecords()
    {
        _allRecords = await API.Instance.GetRecords();
        Records.Clear();
        _allRecords.ForEach(r => Records.Add(r));
    }

    private async Task ReloadOrders()
    {
        Orders.Clear();
        (await API.Instance.GetOrders()).ForEach(o => Orders.Add(o));
    }

    private async Task ReloadProducts()
    {
        Products.Clear();
        (await API.Instance.GetProducts()).ForEach(p => Products.Add(p));
    }

    private async Task ReloadManufacturers()
    {
        Manufacturers.Clear();
        (await API.Instance.GetManufacturers()).ForEach(m => Manufacturers.Add(m));
    }

    private async Task ReloadProductTypes()
    {
        ProductTypes.Clear();
        (await API.Instance.GetProductTypes()).ForEach(t => ProductTypes.Add(t));
    }

    private async Task ReloadServiceTypes()
    {
        ServiceTypes.Clear();
        (await API.Instance.GetServiceTypes()).ForEach(s => ServiceTypes.Add(s));
    }

    // ── Вспомогательный метод открытия окон ──────────────────────────────────

    private void OpenWindow(Window window, Func<Task>? onClose = null)
    {
        var parent = (Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (onClose != null)
            window.Closed += async (_, _) => await onClose();
        window.ShowDialog(parent);
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e) =>
        NavigationService.Navigate(new MainDesktop());
}