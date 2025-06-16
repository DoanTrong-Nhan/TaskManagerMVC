using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TaskManagerMVC.DBContext;
using TaskManagerMVC.Middleware;
using TaskManagerMVC.Repositories.Imp;
using TaskManagerMVC.Repositories.Interfaces;
using TaskManagerMVC.Scanner;
using TaskManagerMVC.Services.Imp;
using TaskManagerMVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Load cấu hình từ secrets và environment
builder.Configuration.AddUserSecrets<Program>();
builder.Configuration.AddEnvironmentVariables();

// Add MVC
builder.Services.AddControllersWithViews();

// Cấu hình Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

// Đăng ký DbContext
builder.Services.AddDbContext<TaskManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();

// Đăng ký các services và repositories
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<PermissionSeeder>();

// Cấu hình logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// Seed quyền khi khởi chạy
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<PermissionSeeder>();
    seeder.SeedPermissions(); // Nên xử lý ngoại lệ ở đây nếu SeedPermissions có thể ném lỗi
}

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Dùng UseExceptionHandler như một fallback
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Di chuyển xuống sau UseAuthorization
app.UseMiddleware<PermissionMiddleware>();

// Cấu hình định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();