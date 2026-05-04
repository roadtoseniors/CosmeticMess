using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;

namespace CosmeticMess.Views.Desktop;

public partial class ManufacturerEditWindow : Window
{
    public ObservableCollection<Country> Countries { get; set; } = new();
    private readonly Manufacturer _manufacturer;
    private readonly bool _isNew;

    public ManufacturerEditWindow(Manufacturer manufacturer, bool isNew)
    {
        _manufacturer = manufacturer;
        _isNew = isNew;
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private async void Load()
    {
        (await API.Instance.GetCountries()).ForEach(c => Countries.Add(c));
        TitleText.Text = _isNew ? "Новый производитель" : "Редактирование";
        if (!_isNew)
        {
            NameBox.Text = _manufacturer.Name;
            CountryBox.SelectedItem = Countries.FirstOrDefault(c => c.Id == _manufacturer.CountryId);
        }
    }

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text) || CountryBox.SelectedItem is not Country country)
        {
            ErrorText.Text = "Заполните все поля.";
            ErrorText.IsVisible = true;
            return;
        }

        _manufacturer.Name      = NameBox.Text;
        _manufacturer.CountryId = country.Id;
        _manufacturer.Country   = country;

        if (_isNew) await API.Instance.PostManufacturer(_manufacturer);
        else        await API.Instance.PutManufacturer(_manufacturer);

        Close();
    }
}