using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;

namespace CosmeticMess.Views.Desktop;

public partial class AddRecordWindow : Window
{
    public ObservableCollection<User>  Masters { get; set; } = new();
    public ObservableCollection<ServiceType> ServiceTypes { get; set; } = new();

    private List<User> _allUsers = new();

    public AddRecordWindow()
    {
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private async void Load()
    {
        _allUsers = await API.Instance.GetUsers();
        var serviceTypes = await API.Instance.GetServiceTypes();
        var masterServices = await API.Instance.GetMasterServices();

        // Только мастера — у кого есть услуги
        var masterIds = masterServices.Select(ms => ms.UserId).ToHashSet();
        _allUsers.Where(u => masterIds.Contains(u.Id)).ToList().ForEach(u => Masters.Add(u));
        serviceTypes.ForEach(s => ServiceTypes.Add(s));

        DatePicker.DisplayDateStart = DateTime.Today;
    }

    private void ClientSearch_OnChanged(object? sender, TextChangedEventArgs e)
    {
        var q = ClientSearchBox.Text?.ToLower() ?? "";
        if (string.IsNullOrWhiteSpace(q))
        {
            ClientList.ItemsSource = null;
            return;
        }

        var found = _allUsers.Where(u =>
            (u.Name?.ToLower().Contains(q) ?? false) ||
            (u.LastName?.ToLower().Contains(q) ?? false) ||
            (u.Phone?.Contains(q) ?? false)).ToList();

        ClientList.ItemsSource = found.Select(u => $"{u.Name} {u.LastName} {u.Phone}").ToList();
        ClientList.Tag = found;
    }

    private void ClientList_OnChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ClientList.SelectedIndex < 0) return;
        if (ClientList.Tag is List<User> users && ClientList.SelectedIndex < users.Count)
        {
            var user = users[ClientList.SelectedIndex];
            ClientSearchBox.Text = $"{user.Name} {user.LastName}";
            ClientList.ItemsSource = null;
            ClientList.Tag = user;
        }
    }

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ClientList.Tag is not User client)
        {
            ErrorText.Text = "Выберите клиента из списка.";
            ErrorText.IsVisible = true;
            return;
        }

        if (MasterBox.SelectedItem is not User master ||
            ServiceBox.SelectedItem is not ServiceType service ||
            DatePicker.SelectedDate is null)
        {
            ErrorText.Text = "Заполните все поля.";
            ErrorText.IsVisible = true;
            return;
        }

        var time = TimePicker.SelectedTime ?? TimeSpan.Zero;
        var date = DatePicker.SelectedDate.Value.Date.Add(time);

        var statuses = await API.Instance.GetRecordStatuses();
        var status = statuses.FirstOrDefault() ?? new RecordStatus { Id = 1 };

        var record = new Record
        {
            ClientId      = client.Id,
            Client        = client,
            MasterId      = master.Id,
            Master        = master,
            ServiceTypeId = service.Id,
            ServiceType   = service,
            Date          = date,
            StatusId      = status.Id,
            Status        = status
        };

        await API.Instance.PostRecords(record);
        Close();
    }
}