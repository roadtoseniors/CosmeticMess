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
    
    public record AuthData(User user, string token);
}