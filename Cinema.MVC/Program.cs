using Cinema.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Отримуємо рядок підключення з appsettings.json або використовуємо резервний
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=../Cinema.Console/cinema.db";

// Підключаємо контекст до бази
builder.Services.AddDbContext<CinemaContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Міграція — лише якщо база ще не створена
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CinemaContext>();
    context.Database.Migrate(); // або context.Database.EnsureCreated() — для простого створення
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customers}/{action=Index}/{id?}");

app.Run();
