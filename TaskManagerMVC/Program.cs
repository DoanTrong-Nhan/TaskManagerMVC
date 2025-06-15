using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagerMVC.DBContext;
using TaskManagerMVC.JWT;
using TaskManagerMVC.Middleware;
using TaskManagerMVC.Repositories.Imp;
using TaskManagerMVC.Repositories.Interfaces;
using TaskManagerMVC.Scanner;
using TaskManagerMVC.Services.Imp;
using TaskManagerMVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Load cấu hình bí mật và biến môi trường
builder.Configuration.AddUserSecrets<Program>();
builder.Configuration.AddEnvironmentVariables();

// Add MVC Controllers with Views
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<PermissionSeeder>();

// Đọc cấu hình JWT từ appsettings.json
var jwtSettings = new JwtSetting();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

// Cấu hình Authentication với JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret!)),
        ClockSkew = TimeSpan.Zero // Không cho phép lệch thời gian
    };
});

// Đăng ký DbContext
builder.Services.AddDbContext<TaskManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký các repository và service
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();


// JWT Token Generator
builder.Services.AddScoped<JwtTokenGenerator>();

// Logging (được hỗ trợ sẵn nhưng khai báo rõ)
builder.Services.AddLogging();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<PermissionSeeder>();
    seeder.SeedPermissions();
}


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<JwtFromCookieMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<PermissionMiddleware>();

// Định tuyến Controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
