namespace TaskManagerMVC.Middleware
{
    public class JwtFromCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtFromCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("jwtToken", out var token) &&
                !context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Request.Headers.Append("Authorization", $"Bearer {token}");
            }

            await _next(context);
        }
    }

}
