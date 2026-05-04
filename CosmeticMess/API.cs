using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CosmeticMess.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/auth/login");
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
    
    public async Task<User?> Register(User user)
    {
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/auth/register");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }
        else
        {
            return null;
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

    public async Task<List<RecordStatus>> GetRecordStatuses()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/recordstatuses");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<RecordStatus>? statuses = JsonSerializer.Deserialize<List<RecordStatus>>(await response.Content.ReadAsStringAsync(), options);
            return statuses;
        }
        else
        {
            return new List<RecordStatus>();
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

    public async Task<List<OrderStatus>> GetOrderStatuses()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/orderstatuses");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            List<OrderStatus>? statuses =
                JsonSerializer.Deserialize<List<OrderStatus>>(await response.Content.ReadAsStringAsync(), options);
            return statuses;
        }
        else
        {
            return new List<OrderStatus>();
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

    public async Task<Basket?> GetBasketByUser(int userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5000/api/baskets/user/{userId}");
        var response = await Client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.OK && !string.IsNullOrWhiteSpace(body) && body != "null")
            return JsonSerializer.Deserialize<Basket>(body, options);
        return null;
    }

    public async Task<Basket?> PostBasket(Basket basket)
    {
        var json = JsonSerializer.Serialize(basket);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/baskets");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<Basket>(await response.Content.ReadAsStringAsync(), options);
        }
        else
        {
            return null;
        }
    }

    public async Task<BasketItem?> PostBasketItem(BasketItem basketItem)
    {
        var json = JsonSerializer.Serialize(basketItem);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/basketitems");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<BasketItem>(await response.Content.ReadAsStringAsync(), options);
        }
        else
        {
            return null;
        }
    }

    public async Task<bool> PutBasketItem(BasketItem basketItem)
    {
        var json = JsonSerializer.Serialize(basketItem);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:5000/api/put/basketitems");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> DeleteBasketItem(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5000/api/delete/basketitems?id={id}");
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<OrderItem?> PostOrderItem(OrderItem orderItem)
    {
        var json = JsonSerializer.Serialize(orderItem);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/orderitems");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<OrderItem>(await response.Content.ReadAsStringAsync(), options);
        }
        else
        {
            return null;
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

    public async Task<User?> PostUsers(User user)
    {
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/users");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
        {
            var result = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task<Product?> PostProducts(Product product)
    {
        var json = JsonSerializer.Serialize(product);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/products");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
        {
            var result = JsonSerializer.Deserialize<Product>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task<Order?> PostOrders(Order order)
    {
        var json = JsonSerializer.Serialize(order);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/orders");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
        {
            var result = JsonSerializer.Deserialize<Order>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task<Record?> PostRecords(Record record)
    {
        var json = JsonSerializer.Serialize(record);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/records");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
        {
            var result = JsonSerializer.Deserialize<Record>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task<User?> PutUsers(User user)
    {
        var json = JsonSerializer.Serialize(user);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5000/api/put/users/{user.Id}");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task<Product?> PutProducts(Product product)
    {
        var json = JsonSerializer.Serialize(product);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5000/api/put/products/{product.Id}");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<Product>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task<Record?> PutRecords(Record record)
    {
        var json = JsonSerializer.Serialize(record);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5000/api/put/records");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<Record>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task<Order?> PutOrders(Order order)
    {
        var json = JsonSerializer.Serialize(order);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5000/api/put/orders/{order.Id}");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<Order>(await response.Content.ReadAsStringAsync(), options);
            return result;
        }
        else
        {
            return null;
        }
    }

    public async Task<bool> DeleteUsers(User user)
    {
        var request  = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5000/api/delete/users/{user.Id}");
        var response = await Client.SendAsync(request);
        return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent; 
    }

    public async Task<bool> DeleteOrders(Order order)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5000/api/delete/orders/{order.Id}");
        var response = await Client.SendAsync(request);
        return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<bool> DeleteRecords(Record record)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5000/api/delete/records/{record.Id}");
        var response = await Client.SendAsync(request);
        return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<bool> DeleteProducts(Product product)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5000/api/delete/products/{product.Id}");
        var response = await Client.SendAsync(request);
        return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
    }
    
    public async Task<MasterService?> PostMasterService(MasterService ms)
    {
        var json = JsonSerializer.Serialize(ms);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/masterservices");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<MasterService>(await response.Content.ReadAsStringAsync(), options);
        return null;
    }

    public async Task<bool> DeleteMasterService(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5000/api/delete/masterservices?id={id}");
        var response = await Client.SendAsync(request);
        return response.StatusCode == HttpStatusCode.OK;
    }
    
        public async Task<Manufacturer?> PostManufacturer(Manufacturer m)
    {
        var json = JsonSerializer.Serialize(m);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/manufacturers");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<Manufacturer>(await response.Content.ReadAsStringAsync(), options);
        return null;
    }

    public async Task<Manufacturer?> PutManufacturer(Manufacturer m)
    {
        var json = JsonSerializer.Serialize(m);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:5000/api/put/manufacturers");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<Manufacturer>(await response.Content.ReadAsStringAsync(), options);
        return null;
    }

    public async Task<ServiceType?> PostServiceType(ServiceType s)
    {
        var json = JsonSerializer.Serialize(s);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/servicetypes");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<ServiceType>(await response.Content.ReadAsStringAsync(), options);
        return null;
    }

    public async Task<ServiceType?> PutServiceType(ServiceType s)
    {
        var json = JsonSerializer.Serialize(s);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:5000/api/put/servicetypes");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<ServiceType>(await response.Content.ReadAsStringAsync(), options);
        return null;
    }

    public async Task<ProductType?> PostProductType(ProductType t)
    {
        var json = JsonSerializer.Serialize(t);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/post/producttypes");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<ProductType>(await response.Content.ReadAsStringAsync(), options);
        return null;
    }

    public async Task<ProductType?> PutProductType(ProductType t)
    {
        var json = JsonSerializer.Serialize(t);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:5000/api/put/producttypes");
        request.Content = content;
        var response = await Client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<ProductType>(await response.Content.ReadAsStringAsync(), options);
        return null;
    }
    
    public record AuthData(User user, string token);
}