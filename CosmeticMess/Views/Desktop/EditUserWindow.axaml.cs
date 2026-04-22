using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CosmeticMess.Entities;

namespace CosmeticMess.Views.Desktop;

public partial class EditUserWindow : Window
{
    public ObservableCollection<Role> Roles { get; set; } = new();
    
    private readonly User user;
    private readonly bool isNew;
    public EditUserWindow(User _user, bool _isNew)
    {
        user = _user;
        isNew = _isNew;
        InitializeComponent();
        DataContext = this;
        Load();
    }

    private async void Load()
    {
        var roles = await API.Instance.GetRoles();
        roles.ForEach(r => Roles.Add(r));
        
        TitleText.Text = isNew ? "Новый пользователь" : "Редактирование";
        
        FreezeButton.IsVisible = !isNew;

        if (!isNew)
        {
            NameBox.Text = user.Name;
            LastNameBox.Text = user.LastName;
            PhoneBox.Text = user.Phone;
            AgeBox.Value = user.Age;
            LoginBox.Text = user.Login;
            RoleBox.SelectedItem = Roles.FirstOrDefault(r => r.Id == user.RoleId);
            
            UpdateFreezeButton();
        }
    }
    
    private void UpdateFreezeButton()
    {
        FreezeButton.Content = user.IsFrozen ? "Разморозить" : "Заморозить";
        FreezeButton.Background = user.IsFrozen ? Avalonia.Media.Brush.Parse("#c7e0ff") : Avalonia.Media.Brush.Parse("#ffe2e2");
    }

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text) ||
            string.IsNullOrWhiteSpace(LastNameBox.Text))
        {
            ErrorText.Text = "Имя и фамилия обязательны.";
            ErrorText.IsVisible = true;
            return;
        }

        if (RoleBox.SelectedItem is not Role role)
        {
            ErrorText.Text = "Выберите роль.";
            ErrorText.IsVisible = true;
            return;
        }

        user.Name      = NameBox.Text;
        user.LastName  = LastNameBox.Text;
        user.Phone     = PhoneBox.Text;
        user.Age       = (int?)AgeBox.Value;
        user.Login     = LoginBox.Text;
        user.RoleId    = role.Id;
        user.Role      = role;

        if (isNew)
            await API.Instance.PostUsers(user);
        else
            await API.Instance.PutUsers(user);

        Close();   
    }

    private async void Freeze_OnClick(object? sender, RoutedEventArgs e)
    {
        user.IsFrozen = !user.IsFrozen;
        await API.Instance.PutUsers(user);
        UpdateFreezeButton();
    }
}