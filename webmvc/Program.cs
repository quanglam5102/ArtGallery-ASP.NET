using Microsoft.EntityFrameworkCore;
using webmvc.Models;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var serverName = Environment.GetEnvironmentVariable("DB_SERVER_NAME") ?? "";

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HttpOnly
    options.Cookie.IsEssential = true; // Ensures the session is created even if the user doesn't accept cookies
});

var connectionString = $"Server={serverName};Database=ArtGallery;Trusted_Connection=True;TrustServerCertificate=True";

builder.Services.AddDbContext<ArtContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session middleware here
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
