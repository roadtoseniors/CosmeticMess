using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class SelectedRecord : Page
{
    public Record Record { get; set; }
    public ObservableCollection<PaymentType> PaymentTypes { get; set; } = new();
    public SelectedRecord(Record record)
    {
        Record = record;
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new RecordDesktop());
    }

    private async void Recorded_OnClick(object? sender, RoutedEventArgs e)
    {
        if (PaymentComboBox.SelectedItem is not PaymentType payment)
        {
            ErrorText.IsVisible = true;
            return;
        }
        
        Record.ClientId  = API.Instance.AuthUser.Id;
        Record.Client    = API.Instance.AuthUser;
        Record.PaymentId = payment.Id;
        Record.Payment   = payment;
        Record.Comment   = CommentBox.Text;
        Record.StatusId  = 2;
        
        var result = await API.Instance.PutRecords(Record);
        if (result != null)
            NavigationService.Navigate(new MainDesktop());
        else
            ErrorText.IsVisible = true;
    }

    private async void Load()
    {
        var types = await API.Instance.GetPaymentTypes();
        types.ForEach(t => PaymentTypes.Add(t));
    }
}