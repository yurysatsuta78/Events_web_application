using Application.DTO.Request.Event;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.DTOValidators.Event
{
    public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
    {
        public CreateEventDtoValidator() 
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");

            RuleFor(dto => dto.EventTime)
                .NotEmpty().WithMessage("Event time cannot be empty.")
                .GreaterThan(DateTime.Now).WithMessage("Event time must be in the future.");

            RuleFor(dto => dto.Location)
                .NotEmpty().WithMessage("Location cannot be empty.")
                .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");

            RuleFor(dto => dto.Category)
                .NotEmpty().WithMessage("Category cannot be empty.")
                .MaximumLength(100).WithMessage("Category must not exceed 100 characters.");

            RuleFor(dto => dto.MaxParticipants)
                .NotEmpty().WithMessage("Maximum participants cannot be empty.")
                .InclusiveBetween(1, 99999).WithMessage("Maximum participants must be between 1 and 99999.");

            RuleFor(dto => dto.Images)
                .NotEmpty().WithMessage("Atleast one image must be uploaded.")
                .Must(MustBeImage).WithMessage("Uploaded file must be an image.");
        }


        private bool MustBeImage(IFormFile[] files)
        {
            if (files == null)
            {
                return false;
            }

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".heic" };

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    return false;
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!validExtensions.Contains(extension))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
