using Microsoft.EntityFrameworkCore;
using UDShortURL.Models;


var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext với SQL Server (hoặc bất kỳ provider nào bạn sử dụng)
builder.Services.AddDbContext<DsShortUrlContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký các dịch vụ khác
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ShortURL}/{action=TrangTao}/{id?}");


app.Run();
