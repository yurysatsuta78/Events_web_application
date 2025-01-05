using Application.UseCases.Auth.Login.DTOs;
using FluentValidation;

namespace Application.UseCases.Auth.Login.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
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
