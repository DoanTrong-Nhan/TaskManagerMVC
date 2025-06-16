using System.Security.Claims;
using TaskManagerMVC.Repositories.Interfaces;

namespace TaskManagerMVC.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";

            // ✅ Bỏ qua một số đường dẫn không cần kiểm tra quyền
            if (path.StartsWith("/login"))
            {
                await _next(context);
                return;
            }

            // Nếu chưa đăng nhập
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                // Cho phép truy cập các file tĩnh và trang login
                if (path.StartsWith("/login") ||
                    context.Request.Method == "POST") 
                {
                    await _next(context);
                    return;
                }

                // Chặn tất cả các trang khác
                context.Response.Redirect("/Login/Index");
                return;
            }



            // ✅ Lấy user ID từ claim
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // ✅ Lấy user và quyền từ repository
            var authRepo = context.RequestServices.GetRequiredService<IAuthRepository>();
            var user = await authRepo.GetByIdAsync(userId);
            if (user == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            // ✅ Kiểm tra quyền truy cập endpoint
            var method = context.Request.Method;
            var endpoint = path;

            var hasPermission = user.Role.RolePermissions.Any(rp =>
                rp.Permission.Method.Equals(method, StringComparison.OrdinalIgnoreCase) &&
                endpoint.Contains(rp.Permission.Endpoint, StringComparison.OrdinalIgnoreCase));

            if (!hasPermission)
            {
                context.Response.Redirect("/Login/AccessDenied");
                return;
            }

            // ✅ Có quyền => tiếp tục
            await _next(context);
        }
    }
}
