using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth
{
    public sealed class CreateAdminRequestValidator : AbstractValidator<CreateAdminRequestDto>
    {
        public CreateAdminRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email address is required.")
                .EmailAddress()
                .WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters.")
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one non-alphanumeric character.");
        }
    }
}
