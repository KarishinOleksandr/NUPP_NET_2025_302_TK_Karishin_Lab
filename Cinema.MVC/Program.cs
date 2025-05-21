using Cinema.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// �������� ����� ���������� � appsettings.json ��� ������������� ���������
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=../Cinema.Console/cinema.db";

// ϳ�������� �������� �� ����
builder.Services.AddDbContext<CinemaContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ̳������ � ���� ���� ���� �� �� ��������
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CinemaContext>();
    context.Database.Migrate(); // ��� context.Database.EnsureCreated() � ��� �������� ���������
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
