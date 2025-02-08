using System.Reflection;
using System.Text;
using BasicOrderSystem.Api.Extensions;
using BasicOrderSystem.Application;
using BasicOrderSystem.Domain.Entities.cs;
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
    .AddUserIdentity(builder.Configuration)
    .AddEndpoints(Assembly.GetExecutingAssembly());


var app = builder.Build();

app.MapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();


app.Run();
