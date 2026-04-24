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

public partial class OrderWindow : Window
{
    public ObservableCollection<PaymentType> PaymentTypes { get; set; } = new();
    private readonly List<BasketItem> _items;
    private readonly Basket _basket;
    public OrderWindow(List<BasketItem> items, Basket basket)
    {
        _items = items;
        _basket = basket;
        InitializeComponent();
        DataContext = this;
        
        DeliveryDatePicker.DisplayDateStart = DateTime.Today.AddDays(1);
        DeliveryDatePicker.DisplayDateEnd = DateTime.Today.AddDays(7);

        Load();
    }

    private async void Load()
    {
        var types = await API.Instance.GetPaymentTypes();
        types.ForEach(t => PaymentTypes.Add(t));
    }
    
    

    private async void Confirm_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DeliveryDatePicker.SelectedDate is null)
        {
            ErrorText.Text = "Выберите дату получения.";
            ErrorText.IsVisible = true;
            return;
        }

        if (PaymentComboBox.SelectedItem is not PaymentType payment)
        {
            ErrorText.Text = "Выберите способ оплаты.";
            ErrorText.IsVisible = true;
            return;
        }

        var statuses = await API.Instance.GetOrderStatuses();
        var status = statuses.FirstOrDefault() ?? new OrderStatus { Id = 1 };
        var user = API.Instance.AuthUser;

        var order = new Order
        {
            UserId = user.Id,
            User = user,
            Date = DateTime.Now,
            DeliveryDate = DateOnly.FromDateTime(DeliveryDatePicker.SelectedDate.Value.Date),
            PaymentId = payment.Id,
            Payment = payment,
            StatusId = status.Id,
            Status = status
        };

        var createdOrder = await API.Instance.PostOrders(order);
        if (createdOrder == null)
        {
            ErrorText.Text = "Ошибка при создании заказа.";
            ErrorText.IsVisible = true;
            return;
        }

        foreach (var item in _items)
        {
            await API.Instance.PostOrderItem(new OrderItem
            {
                OrderId = createdOrder.Id,
                Order = createdOrder,
                ProductId = item.Product.Id,
                Product = item.Product,
                Quantity = item.Quantity,
                PriceOrder = item.Product.Price
            });

            await API.Instance.DeleteBasketItem(item.Id);
        }

        Close();
    }
}