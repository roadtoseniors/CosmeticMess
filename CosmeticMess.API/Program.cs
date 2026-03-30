
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CosmeticMess.Context;
using CosmeticMess.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddCors();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AuthOptions.ISSUER,
        ValidateAudience = true,
        ValidAudience = AuthOptions.AUDIENCE,
        ValidateLifetime =  true,
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    };
});

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/auth/login", (AuthData data, MyDbContext cnt) =>
{
    User user = cnt.Users.FirstOrDefault(u => u.Login == data.Login && u.Password == data.Password);
    if (user != null)
    {
        var claims = new List<Claim> {new Claim(ClaimTypes.Name, data.Login)};

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(24)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        return Results.Ok(new {User = user, Token = new JwtSecurityTokenHandler().WriteToken(jwt)});
    }
    else
    {
        return Results.Unauthorized();
    }
});

app.MapGet("/api/users", (MyDbContext cnt) =>{
    return cnt.Users.ToList();
});

app.MapGet("/api/servicetypes", (MyDbContext cnt) =>
{
    return cnt.ServiceTypes.ToList();
});

app.MapGet("/api/roles", (MyDbContext cnt) =>
{
    return cnt.Roles.ToList();
});

app.MapGet("/api/recordstatuses", (MyDbContext cnt) =>
{
    return cnt.RecordStatuses.ToList();
});

app.MapGet("/api/records", (MyDbContext cnt) =>
{
    return cnt.Records.ToList();
});

app.MapGet("/api/producttypes", (MyDbContext cnt) =>
{
    return cnt.ProductTypes.ToList();
});

app.MapGet("/api/products", (MyDbContext cnt) =>
{
    return cnt.Products.ToList();
});

app.MapGet("/api/paymenttypes", (MyDbContext cnt) =>
{
    return cnt.PaymentTypes.ToList();
});

app.MapGet("/api/orderstatuses", (MyDbContext cnt) =>
{
    return cnt.OrderStatuses.ToList();
});

app.MapGet("/api/orderitems", (MyDbContext cnt) =>
{
    return cnt.OrderItems.ToList();
});

app.MapGet("/api/orders", (MyDbContext cnt) =>
{
    return cnt.Orders.ToList();
});

app.MapGet("/api/masterservices", (MyDbContext cnt) =>
{
    return cnt.MasterServices.ToList();
});

app.MapGet("/api/manufacturers", (MyDbContext cnt) =>
{
    return cnt.Manufacturers.ToList();
});

app.MapGet("/api/countries", (MyDbContext cnt) =>
{
    return cnt.Countries.ToList();
});

app.MapGet("/api/basketitems", (MyDbContext cnt) =>
{
    return cnt.BasketItems.ToList();
});

app.MapGet("/api/baskets", (MyDbContext cnt) =>
{
    return cnt.Baskets.ToList();
});

app.Run();

public class AuthOptions
{
    public const string ISSUER = "Masho";
    public const string AUDIENCE = "Vava";
    private const string KEY = "Baton12345";

    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}

public record AuthData(string Login, string Password);