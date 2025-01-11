using Application.UseCases.Events.Update.DTOs;
using FluentValidation;

namespace Application.UseCases.Events.Update.Validators
{
    public class AddEventParticipantRequestValidator : AbstractValidator<AddEventParticipantRequest>
    {
        public AddEventParticipantRequestValidator() 
        {
            RuleFor(x => x.EventId)
                .NotEmpty().WithMessage("Event ID cannot be empty.");

            RuleFor(x => x.ParticipantId)
                .NotEmpty().WithMessage("Participant ID cannot be empty.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("Participant ID must be a valid GUID.");
        }
    }
}
