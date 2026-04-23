using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using CosmeticMess.Entities;
using WpfLikeAvaloniaNavigation;

namespace CosmeticMess.Views.Desktop;

public partial class AdminDesktop : Page
{
    private ObservableCollection<User> _allUsers = new();
    private string _searchQuery = "";

    public AdminDesktop()
    {
        InitializeComponent();
        Load();
    }


    private async void Load()
    {
        await Reload();
    }

    private async Task Reload()
    {
        _allUsers.Clear();
        var users = await API.Instance.GetUsers();
        users.ForEach(u => _allUsers.Add(u));
        ApplyFilter();
    }


    private void ApplyFilter()
    {
        var filtered = string.IsNullOrWhiteSpace(_searchQuery) ? _allUsers.ToList() : _allUsers.Where(u =>
                (u.Name + " " + u.LastName).Contains(_searchQuery, System.StringComparison.OrdinalIgnoreCase) ||
                (u.Login ?? "").Contains(_searchQuery, System.StringComparison.OrdinalIgnoreCase)).ToList();

        UsersList.ItemsSource = filtered;
        CountText.Text = $"{filtered.Count} пользователей";
    }
    

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new MainDesktop());
    }


    private async void Frozen_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not User user) return;
        user.IsFrozen = !user.IsFrozen;
        await API.Instance.PutUsers(user);
        ApplyFilter();
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenEditWindow(new User(), isNew: true);
    }

    private void Edit_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is User user)
            OpenEditWindow(user, isNew: false);
    }

    private void OpenEditWindow(User user, bool isNew)
    {
        var window = new EditUserWindow(user, isNew);
        window.Closed += async (_, _) => await Reload();
        var parent = (Avalonia.Application.Current?.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime)?.MainWindow;
        window.ShowDialog(parent);
    }
}