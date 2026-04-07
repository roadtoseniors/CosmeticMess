using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CosmeticMess.Entities;

namespace CosmeticMess;


internal class API
{
    private static API _instance = new API();
    public static API Instance => _instance;

    public User AuthUser;

    private API()
    {
        Client = new HttpClient();
    }
    
    public readonly HttpClient Client;

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    public void SetupJWTToken(string token)
    {
        Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }
    

    public async Task<bool> Auth(string login, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/auth/login");
        var json = JsonSerializer.Serialize(new {Login = login, Password = password});
        var content = new StringContent(json, null, "application/json");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<AuthData>(await response.Content.ReadAsStringAsync(), options);
            SetupJWTToken(result.token);
            AuthUser = result.user;
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<List<User>> GetUsers()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/users");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<User>? users = JsonSerializer.Deserialize<List<User>>(await response.Content.ReadAsStringAsync(), options);
            return users;
        }
        else
        {
            return new List<User>();
        }
    }

    public async Task<List<ServiceType>> GetServiceTypes()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/servicetypes");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<ServiceType>? serviceTypes = JsonSerializer.Deserialize<List<ServiceType>>(await response.Content.ReadAsStringAsync(), options);
            return serviceTypes;
        }
        else
        {
            return new List<ServiceType>();
        }
    }

    public async Task<List<Role>> GetRoles()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/roles");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<Role>? roles = JsonSerializer.Deserialize<List<Role>>(await response.Content.ReadAsStringAsync(), options);
            return roles;
        }
        else
        {
            return new List<Role>();
        }
    }

    public async Task<List<Record>> GetRecords()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/records");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<Record>? records = JsonSerializer.Deserialize<List<Record>>(await response.Content.ReadAsStringAsync(), options);
            return records;
        }
        else
        {
            return new List<Record>();
        }
    }

    public async Task<List<ProductType>> GetProductTypes()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/producttypes");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<ProductType>? productTypes = JsonSerializer.Deserialize<List<ProductType>>(await response.Content.ReadAsStringAsync(), options);
            return productTypes;
        }
        else
        {
            return new List<ProductType>();
        }
    }

    public async Task<List<Product>> GetProducts()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/products");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<Product>? products = JsonSerializer.Deserialize<List<Product>>(await response.Content.ReadAsStringAsync(), options);
            return products;
        }
        else
        {
            return new List<Product>();
        }
    }

    public async Task<List<PaymentType>> GetPaymentTypes()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/paymenttypes");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<PaymentType>? paymentTypes = JsonSerializer.Deserialize<List<PaymentType>>(await response.Content.ReadAsStringAsync(), options);
            return paymentTypes;
        }
        else
        {
            return new List<PaymentType>();
        }
    }

    public async Task<List<OrderItem>> GetOrderItems()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/orderitems");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<OrderItem>? orderItems = JsonSerializer.Deserialize<List<OrderItem>>(await response.Content.ReadAsStringAsync(), options);
            return orderItems;
        }
        else
        {
            return new List<OrderItem>();
        }
    }

    public async Task<List<Order>> GetOrders()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/orders");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<Order>? orders = JsonSerializer.Deserialize<List<Order>>(await response.Content.ReadAsStringAsync(), options);
            return orders;
        }
        else
        {
            return new List<Order>();
        }
    }

    public async Task<List<Manufacturer>> GetManufacturers()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/manufacturers");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<Manufacturer>? manufacturers = JsonSerializer.Deserialize<List<Manufacturer>>(await response.Content.ReadAsStringAsync(), options);
            return manufacturers;
        }
        else
        {
            return new List<Manufacturer>();
        }
    }

    public async Task<List<Country>> GetCountries()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/countries");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<Country>? countries = JsonSerializer.Deserialize<List<Country>>(await response.Content.ReadAsStringAsync(), options);
            return countries;
        }
        else
        {
            return new List<Country>();
        }
    }

    public async Task<List<BasketItem>> GetBasketItems()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/basketitems");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<BasketItem>? basketItems = JsonSerializer.Deserialize<List<BasketItem>>(await response.Content.ReadAsStringAsync(), options);
            return basketItems;
        }
        else
        {
            return new List<BasketItem>();
        }
    }

    public async Task<List<Basket>> GetBaskets()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/baskets");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<Basket>? baskets = JsonSerializer.Deserialize<List<Basket>>(await response.Content.ReadAsStringAsync(), options);
            return baskets;
        }
        else
        {
            return new List<Basket>();
        }
    }

    public async Task<List<MasterService>> GetMasterServices()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/masterservices");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<MasterService> masterServices =
                JsonSerializer.Deserialize<List<MasterService>>(await response.Content.ReadAsStringAsync(), options);
            return masterServices;
        }
        else
        {
            return new List<MasterService>();
        }
    }
    
    public record AuthData(User user, string token);
}