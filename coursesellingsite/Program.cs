using Microsoft.EntityFrameworkCore;
using coursesellingsite.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext with connection string from appsettings.json
builder.Services.AddDbContext<DbContextCourse>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection")));

// Add Session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Cookies can only be accessed via HTTP(S)
    options.Cookie.IsEssential = true; // Essential cookies for session
});

// Register your services if needed (e.g., CartService, etc.)
// builder.Services.AddScoped<ICartService, CartService>();

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

// Enable session middleware to handle session state
app.UseSession();

// Use authorization if you need authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
