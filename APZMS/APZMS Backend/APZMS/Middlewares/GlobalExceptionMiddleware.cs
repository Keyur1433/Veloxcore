using APZMS.Domain.Exceptions;
using System.Text.Json;

namespace APZMS.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Global exception caught: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    DomainException => StatusCodes.Status404NotFound,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    InvalidAgeRangeException => StatusCodes.Status400BadRequest,
                    ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
                    InvalidOperationException => StatusCodes.Status500InternalServerError,
                    EmailAlreadyExistsException => StatusCodes.Status409Conflict,
                    BadHttpRequestException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError,
                };

                var errorResponse = new
                {
                    message = context.Response.StatusCode == 500 ? "An unexpected error occurred." : ex.Message,
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }

        }
    }
}
