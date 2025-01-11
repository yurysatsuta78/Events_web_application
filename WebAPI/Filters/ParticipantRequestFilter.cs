using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class ParticipantRequestFilter<TRequest> : IAsyncActionFilter where TRequest : class
    {
        private readonly IValidator<TRequest> _validator;

        public ParticipantRequestFilter(IValidator<TRequest> validator) 
        {
            _validator = validator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            var eventId = Guid.Parse(context.RouteData.Values["eventId"]?.ToString() 
                ?? throw new NotFoundException("Event ID is missing."));

            var participantId = context.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "participantId")?.Value
                ?? throw new UnauthorizedException("Participant ID is missing.");

            var request = Activator.CreateInstance(typeof(TRequest), eventId, participantId) as TRequest
                ?? throw new InvalidOperationException("Failed to create request object.");

            await _validator.ValidateAndThrowAsync(request);

            context.HttpContext.Items["Request"] = request;

            await next();
        }
    }
}
