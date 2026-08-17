using Application.DTOs.Identity;
using FluentValidation;

namespace Application.FluentValidators.Identity
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.UserName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must exceed 3 characters.");

            RuleFor(x => x.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Email is required.")
                .EmailAddress();
        }
    }
}
