using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class MasterDesktop : Page
{
    public ObservableCollection<User> Users { get; } = new();
    public MasterDesktop()
    {
        InitializeComponent();
        DataContext = this;
        
    }

    private async void Load()
    {
        
    }
}