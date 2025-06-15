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

            if (path.StartsWith("/login") || path.StartsWith("/register"))
            {
                await _next(context);
                return;
            }

            var _authRepository = context.RequestServices.GetRequiredService<IAuthRepository>();

            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var user = _authRepository.GetById(userId);
            if (user?.Role?.RolePermissions == null)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            var method = context.Request.Method;
            var endpoint = context.Request.Path.Value?.ToLowerInvariant() ?? "";

            var hasPermission = user.Role.RolePermissions.Any(rp =>
                rp.Permission.Method.Equals(method, StringComparison.OrdinalIgnoreCase) &&
                endpoint.Contains(rp.Permission.Endpoint, StringComparison.OrdinalIgnoreCase));

            if (!hasPermission)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            await _next(context);
        }
    }
}
