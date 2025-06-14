using Microsoft.EntityFrameworkCore;
using TaskManagerMVC.DBContext;
using TaskManagerMVC.Middleware;
using TaskManagerMVC.Repositories.Imp;
using TaskManagerMVC.Repositories.Interfaces;
using TaskManagerMVC.Services.Imp;
using TaskManagerMVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
// Add services to the container.
builder.Services.AddControllersWithViews();

// Thêm cấu hình từ môi trường
builder.Configuration.AddEnvironmentVariables();

// Lấy cấu hình từ appsettings.json (đã được tự load bởi builder.Configuration)
var configuration = builder.Configuration;

// Đăng ký DbContext
builder.Services.AddDbContext<TaskManagerDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Đăng ký các dịch vụ
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Đăng ký ILogger (đã được cung cấp mặc định, không cần thêm dòng này, chỉ để rõ ràng)
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Hiển thị chi tiết lỗi trong môi trường Development
}
else
{
    // Sử dụng ExceptionHandlingMiddleware trong môi trường Production
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=ListTask}/{id?}");

app.Run();