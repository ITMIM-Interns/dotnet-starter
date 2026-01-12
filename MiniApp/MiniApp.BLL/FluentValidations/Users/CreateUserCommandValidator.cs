using FluentValidation;
using MiniApp.BLL.Features.Commands.Users.Create;
using MiniApp.BLL.Helpers;

namespace MiniApp.BLL.FluentValidations.Users
{
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Email).NotEmpty().WithMessage("User email is required").EmailAddress().WithMessage("Email format is wrong")
                .MaximumLength(200).WithMessage("Email cannot exceed 200 characters");

            RuleFor(u => u.Username).NotEmpty().WithMessage("Username is required").MaximumLength(50).WithMessage("Username cannot exceed 50 characters");

            RuleFor(u => u.Password).NotEmpty().WithMessage("Password is required").MaximumLength(45).WithMessage("Password cannot exceed 45 characters")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[!@#$%^&*()_+\-]").WithMessage("Password must contain at least one special character");
            RuleFor(u => u.ContactNumber).NotEmpty().WithMessage("Contact number is required").Matches(@"^\+?[0-9]{10,15}$").WithMessage("Contact number must be a valid phone number");
            RuleFor(u => u.Image).Must(img => img is null || ImageService.IsImage(img)).WithMessage("Uploaded file must be a valid image (jpg, png) ");
        }
    }
}
