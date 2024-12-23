using Application.DTO;
using Domain.Exceptions;
using FluentValidation;
using System.Net;

namespace WebAPI.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
                _logger.LogError(ex, "An unhandled exception has occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred.");

            context.Response.ContentType = "application/json";
            int statusCode;
            string exceptionMessage;

            switch (exception) 
            {
                case InvalidTokenException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    exceptionMessage = exception.Message;
                    break;
                case EntityNotFoundException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    exceptionMessage = exception.Message;
                    break;
                case InvalidPasswordException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    exceptionMessage = exception.Message;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    exceptionMessage = exception.Message    ;
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                message = "An unexpected error occurred.",
                exceptionMessage,
                exceptionDetails = exception.StackTrace
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
