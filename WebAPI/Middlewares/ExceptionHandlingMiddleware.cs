using Application.Exceptions;
using Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;
using WebAPI.Models;

namespace WebAPI.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var (statusCode, errorCode) = exception switch
            {
                NotFoundException => (HttpStatusCode.NotFound, "NOT_FOUND"),

                AlreadyExistsException => (HttpStatusCode.Conflict, "ALREADY_EXISTS"), 

                BusinessException => (HttpStatusCode.BadRequest, "BUSINESS_ERROR"), 

                IdentityException => (HttpStatusCode.BadRequest, "IDENTITY_ERROR"),

                CustomValidationException =>  (HttpStatusCode.BadRequest, "VALIDATION_ERROR"),

                _ => (HttpStatusCode.InternalServerError, "INTERNAL_SERVER_ERROR")
            };

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(exception, "Unhandled exception occurred.");
            }

            Dictionary<string, string[]>? errors = null;

            if (exception is CustomValidationException validationException)
            {
                errors = validationException.Errors
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value);
            }

            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = statusCode == HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred."
                    : exception.Message,
                ErrorCode = errorCode,
                Errors = errors
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
