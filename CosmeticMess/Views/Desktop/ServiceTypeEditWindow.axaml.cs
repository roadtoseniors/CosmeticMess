using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;

namespace CosmeticMess.Views.Desktop;

public partial class ServiceTypeEditWindow : Window
{
    private readonly ServiceType _serviceType;
    private readonly bool _isNew;

    public ServiceTypeEditWindow(ServiceType serviceType, bool isNew)
    {
        _serviceType = serviceType;
        _isNew = isNew;
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        TitleText.Text = _isNew ? "Новая услуга" : "Редактирование услуги";
        if (!_isNew)
        {
            NameBox.Text   = _serviceType.Name;
            PriceBox.Value = (decimal)_serviceType.Price;
        }
    }

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            ErrorText.Text = "Введите название.";
            ErrorText.IsVisible = true;
            return;
        }

        _serviceType.Name  = NameBox.Text;
        _serviceType.Price = (decimal)(PriceBox.Value ?? 0);

        if (_isNew) await API.Instance.PostServiceType(_serviceType);
        else        await API.Instance.PutServiceType(_serviceType);

        Close();
    }
}