using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;

namespace CosmeticMess.Views.Desktop;

public partial class RescheduleRecordWindow : Window
{
    private readonly Record _record;

    public RescheduleRecordWindow(Record record)
    {
        _record = record;
        InitializeComponent();
        DatePicker.DisplayDateStart = DateTime.Today;
        DatePicker.SelectedDate = _record.Date;
        TimePicker.SelectedTime = _record.Date.TimeOfDay;
    }

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DatePicker.SelectedDate is null)
        {
            ErrorText.Text = "Выберите дату.";
            ErrorText.IsVisible = true;
            return;
        }

        var time = TimePicker.SelectedTime ?? TimeSpan.Zero;
        _record.Date = DatePicker.SelectedDate.Value.Date.Add(time);

        await API.Instance.PutRecords(_record);
        Close();
    }
}