using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace TaskManagerMVC.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred in the application.");

            var (statusCode, message) = GetErrorResponse(ex);
            var encodedMessage = WebUtility.UrlEncode(message);

            context.Response.Redirect($"/Home/Error?statusCode={statusCode}&message={encodedMessage}");
        }

        private (int StatusCode, string Message) GetErrorResponse(Exception ex) => ex switch
        {
            ArgumentNullException ane => (StatusCodes.Status400BadRequest, $"Invalid request: {ane.Message}"),
            ArgumentException ae => (StatusCodes.Status400BadRequest, $"Invalid request: {ae.Message}"),
            KeyNotFoundException knfe => (StatusCodes.Status404NotFound, knfe.Message),
            UnauthorizedAccessException uae => (StatusCodes.Status401Unauthorized, $"Unauthorized access: {uae.Message}"),
            InvalidOperationException ioe => (StatusCodes.Status500InternalServerError, $"Server error: {ioe.Message}"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.")
        };
    }
}
