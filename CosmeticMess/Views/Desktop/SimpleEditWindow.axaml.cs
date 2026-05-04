using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace CosmeticMess.Views.Desktop;

public partial class SimpleEditWindow : Window
{
    private readonly Func<string, Task> _onSave;

    public SimpleEditWindow(string title, string currentValue, Func<string, Task> onSave)
    {
        _onSave = onSave;
        InitializeComponent();
        TitleText.Text = title;
        NameBox.Text = currentValue;
    }

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            ErrorText.Text = "Введите название.";
            ErrorText.IsVisible = true;
            return;
        }

        await _onSave(NameBox.Text);
        Close();
    }
}