using Application.UseCases.Events.Update.DTOs;
using FluentValidation;

namespace Application.UseCases.Events.Update.Validators
{
    public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
    {
        public UpdateEventRequestValidator()
        {
            RuleFor(ev => ev.Name)
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(ev => ev.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");

            RuleFor(ev => ev.EventTime)
                .GreaterThan(DateTime.Now).WithMessage("Event time must be in the future.");

            RuleFor(ev => ev.Location)
                .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");

            RuleFor(ev => ev.Category)
                .MaximumLength(100).WithMessage("Category must not exceed 100 characters.");

            RuleFor(ev => ev.MaxParticipants)
                .InclusiveBetween(1, 99999).WithMessage("Maximum participants must be between 1 and 99999.");
        }
    }
}
