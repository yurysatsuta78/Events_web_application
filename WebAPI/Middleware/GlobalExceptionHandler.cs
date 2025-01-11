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

            switch (exception) 
            {
                case AlreadyExistsException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case BadRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case ValidationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                exceptionMessage = exception.Message,
                exceptionDetails = exception.StackTrace
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
