
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CosmeticMess.Context;
using CosmeticMess.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(365)),
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

app.MapGet("/api/roles", [Authorize](MyDbContext cnt) =>
{
    return cnt.Roles.ToList();
});

app.MapGet("/api/recordstatuses", [Authorize](MyDbContext cnt) =>
{
    return cnt.RecordStatuses.ToList();
});

app.MapGet("/api/records", [Authorize](MyDbContext cnt) =>
{
    return cnt.Records.ToList();
});

app.MapGet("/api/producttypes", [Authorize](MyDbContext cnt) =>
{
    return cnt.ProductTypes.ToList();
});

app.MapGet("/api/products", [Authorize](MyDbContext cnt) =>
{
    return cnt.Products.ToList();
});

app.MapGet("/api/paymenttypes", [Authorize](MyDbContext cnt) =>
{
    return cnt.PaymentTypes.ToList();
});

app.MapGet("/api/orderstatuses", [Authorize](MyDbContext cnt) =>
{
    return cnt.OrderStatuses.ToList();
});

app.MapGet("/api/orderitems", [Authorize](MyDbContext cnt) =>
{
    return cnt.OrderItems.ToList();
});

app.MapGet("/api/orders", [Authorize](MyDbContext cnt) =>
{
    return cnt.Orders.ToList();
});

app.MapGet("/api/masterservices", (MyDbContext cnt) =>
{
    return cnt.MasterServices.ToList();
});

app.MapGet("/api/manufacturers", [Authorize](MyDbContext cnt) =>
{
    return cnt.Manufacturers.ToList();
});

app.MapGet("/api/countries", [Authorize](MyDbContext cnt) =>
{
    return cnt.Countries.ToList();
});

app.MapGet("/api/basketitems", [Authorize](MyDbContext cnt) =>
{
    return cnt.BasketItems.ToList();
});

app.MapGet("/api/baskets", [Authorize](MyDbContext cnt) =>
{
    return cnt.Baskets.ToList();
});

app.MapPost("/api/post/users", [Authorize](User user, MyDbContext cnt) =>
{
    cnt.Attach(user.Role);
    cnt.Users.Add(user);
    cnt.SaveChanges();
    return Results.Ok(user);
});

app.MapPost("/api/post/products", [Authorize](Product product, MyDbContext cnt) =>
{
    cnt.Attach(product.ProductType);
    cnt.Attach(product.Manufacturer);
    cnt.Products.Add(product);
    cnt.SaveChanges();
    return Results.Ok(product);
});

app.MapPost("/api/post/records", [Authorize](Record record, MyDbContext cnt) =>
{
    cnt.Attach(record.Master);
    cnt.Attach(record.Client);
    cnt.Attach(record.ServiceType);
    cnt.Attach(record.Payment);
    cnt.Attach(record.Status);
    cnt.Records.Add(record);
    cnt.SaveChanges();
    return Results.Ok(record);
});

app.MapPost("/api/post/orders", [Authorize](Order order, MyDbContext cnt) =>
{
    cnt.Attach(order.Payment);
    cnt.Attach(order.User);
    cnt.Attach(order.Status);
    cnt.Orders.Add(order);
    cnt.SaveChanges();
    return Results.Ok(order);
});

app.MapPut("/api/put/users", [Authorize](User user, MyDbContext cnt) =>
{
    cnt.Users.Update(user);
    cnt.SaveChanges();
    return Results.Ok(user);
});

app.MapPut("/api/put/products", [Authorize](Product product, MyDbContext cnt) =>
{
    cnt.Products.Update(product);
    cnt.SaveChanges();
    return Results.Ok(product);
});

app.MapPut("/api/put/orders", [Authorize](Order order, MyDbContext cnt) =>
{
    cnt.Orders.Update(order);
    cnt.SaveChanges();
    return Results.Ok(order);
});

app.MapPut("/api/put/records", [Authorize](Record record, MyDbContext cnt) =>
{
    cnt.Records.Update(record);
    cnt.SaveChanges();
    return Results.Ok(record);
});

app.MapDelete("/api/delete/users", [Authorize](int id, MyDbContext cnt) =>
{
    User user = cnt.Users.FirstOrDefault(u => u.Id == id);
    if (user != null)
    {
        cnt.Users.Remove(user);
        cnt.SaveChanges();
        return Results.Ok();
    }
    else
    {
        return Results.BadRequest();
    }
});

app.MapDelete("/api/delete/orders", [Authorize](int id, MyDbContext cnt) =>
{
    Order order = cnt.Orders.FirstOrDefault(o => o.Id == id);
    if (order != null)
    {
        cnt.Orders.Remove(order);
        cnt.SaveChanges();
        return Results.Ok();
    }
    else
    {
        return Results.BadRequest();
    }
});

app.MapDelete("/api/delete/products", [Authorize](int id, MyDbContext cnt) =>
{
    Product product = cnt.Products.FirstOrDefault(p => p.Id == id);
    if (product != null)
    {
        cnt.Products.Remove(product);
        cnt.SaveChanges();
        return Results.Ok();
    }
    else
    {
        return Results.BadRequest();
    }
});

app.MapDelete("/api/delete/records", [Authorize](int id, MyDbContext cnt) =>
{
    Record record = cnt.Records.FirstOrDefault(r => r.Id == id);
    if (record != null)
    {
        cnt.Records.Remove(record);
        cnt.SaveChanges();
        return Results.Ok();
    }
    else
    {
        return Results.BadRequest();
    }
});

app.Run();

public class AuthOptions
{
    public const string ISSUER = "Masho";
    public const string AUDIENCE = "Vava";
    private const string KEY = "BatonBatonBatonBatonBatonBaton_BatonBatonBaton123!";

    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}

public record AuthData(string Login, string Password);