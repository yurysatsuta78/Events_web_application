using Application.DTO.Request.Participant;
using FluentValidation;

namespace Application.DTOValidators.Participant
{
    public class CreateParticipantDtoValidator : AbstractValidator<CreateParticipantDto>
    {
        public CreateParticipantDtoValidator() 
        {
            RuleFor(pa => pa.Name)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(pa => pa.Surname)
                .NotEmpty().WithMessage("Surname cannot be empty.")
                .MaximumLength(50).WithMessage("Surname must not exceed 50 characters.");

            RuleFor(pa => pa.Birthday)
                .NotEmpty().WithMessage("Birthday cannot be empty.")
                .LessThan(DateTime.Now).WithMessage("Birthday must be in the past.");

            RuleFor(pa => pa.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .MaximumLength(50).WithMessage("Email must not exceed 50 characters")
                .EmailAddress().WithMessage("Email must be email.");

            RuleFor(pa => pa.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters");
        }
    }
}
