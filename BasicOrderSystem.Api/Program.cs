using System.Reflection;
using System.Text;
using BasicOrderSystem.Api.Extensions;
using BasicOrderSystem.Application;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Infrastructure;
using BasicOrderSystem.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


// Ensure Kestrel listens to both HTTP and HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5150); // HTTP
    options.ListenAnyIP(7295, listenOptions => listenOptions.UseHttps()); // HTTPS
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

app.Use(async (context, next) =>
{
    if (context.User.Identity is { IsAuthenticated: false })
    {
        Console.WriteLine("User is NOT authenticated");
    }
    else
    {
        Console.WriteLine($"User is authenticated as {context.User.Identity?.Name}");
    }

    await next();
});

app.Use(async (context, next) =>
{
    Console.WriteLine("Incoming Headers:");
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"{header.Key}: {header.Value}");
    }

    await next();
});


await app.RunAsync();

public partial class Program;