using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;

namespace CosmeticMess.Views.Desktop;

public partial class ProductEditWindow : Window
{
    public ObservableCollection<ProductType>  ProductTypes  { get; set; } = new();
    public ObservableCollection<Manufacturer> Manufacturers { get; set; } = new();

    private readonly Product _product;
    private readonly bool _isNew;

    public ProductEditWindow(Product product, bool isNew)
    {
        _product = product;
        _isNew = isNew;
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private async void Load()
    {
        (await API.Instance.GetProductTypes()).ForEach(t => ProductTypes.Add(t));
        (await API.Instance.GetManufacturers()).ForEach(m => Manufacturers.Add(m));

        TitleText.Text = _isNew ? "Новый товар" : "Редактирование товара";
        if (!_isNew)
        {
            NameBox.Text       = _product.Name;
            DescBox.Text       = _product.Description;
            PriceBox.Value     = (decimal)_product.Price;
            DiscountBox.Value  = _product.DiscountPercent;
            TypeBox.SelectedItem         = ProductTypes.FirstOrDefault(t => t.Id == _product.ProductTypeId);
            ManufacturerBox.SelectedItem = Manufacturers.FirstOrDefault(m => m.Id == _product.ManufacturerId);
        }
    }

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text) ||
            TypeBox.SelectedItem is not ProductType type ||
            ManufacturerBox.SelectedItem is not Manufacturer manufacturer)
        {
            ErrorText.Text = "Заполните все обязательные поля.";
            ErrorText.IsVisible = true;
            return;
        }

        _product.Name             = NameBox.Text;
        _product.Description      = DescBox.Text;
        _product.Price            = (decimal)(PriceBox.Value ?? 0);
        _product.DiscountPercent  = (int)(DiscountBox.Value ?? 0);
        _product.ProductTypeId    = type.Id;
        _product.ProductType      = type;
        _product.ManufacturerId   = manufacturer.Id;
        _product.Manufacturer     = manufacturer;

        if (_isNew) await API.Instance.PostProducts(_product);
        else        await API.Instance.PutProducts(_product);

        Close();
    }
}