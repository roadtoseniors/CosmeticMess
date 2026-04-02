using Avalonia.Controls;
using CosmeticMess.Views.Desktop;

namespace CosmeticMess.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new MainDesktop());
    }
}