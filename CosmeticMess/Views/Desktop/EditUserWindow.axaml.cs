using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CosmeticMess.Views.Desktop;

public partial class EditUserWindow : Window
{
    public ObservableCollection<Role> Roles { get; set; } = new();

    private readonly User user;
    private readonly bool isNew;
    public EditUserWindow(User user, bool isNew)
    {
        InitializeComponent();
    }

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }
}