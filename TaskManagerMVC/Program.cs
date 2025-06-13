using Microsoft.EntityFrameworkCore;
using TaskManagerMVC.DBContext;
using TaskManagerMVC.Repositories.Imp;
using TaskManagerMVC.Repositories.Interfaces;
using TaskManagerMVC.Services.Imp;
using TaskManagerMVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Thêm cấu hình từ môi trường sớm hơn
builder.Configuration.AddEnvironmentVariables();

// Lấy cấu hình từ appsettings.json ( đã được tự load bởi builder.Configuration)
var configuration = builder.Configuration;

//  Đăng ký DbContext
builder.Services.AddDbContext<TaskManagerDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
