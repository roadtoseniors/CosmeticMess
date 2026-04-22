using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class DetailRecordDesktop : Page
{
    public Record Record { get; set; }
    public DetailRecordDesktop(Record record)
    {
        Record = record;
        InitializeComponent();
        DataContext = this;
        CompleteButton.IsVisible = Record.Status.Name != "Завершено";
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MasterDesktop());
    }

    private async void Complete_OnClick(object? sender, RoutedEventArgs e)
    {
        var statuses = await API.Instance.GetRecordStatuses();
        var completed = statuses.FirstOrDefault(s => s.Name == "Выполнено");
        if (completed is null)
        {
            ErrorText.Text = "Статус 'Выполнено' не найден в базе.";
            ErrorText.IsVisible = true;
            return;
        }

        Record.StatusId = completed.Id;
        Record.Status = completed;

        var result = await API.Instance.PutRecords(Record);
        if (result != null)
        {
            CompleteButton.IsVisible = false;
            ErrorText.IsVisible = false;
        }
        else
        {
            ErrorText.Text = "Не удалось обновить статус.";
            ErrorText.IsVisible = true;
        }
    }
}