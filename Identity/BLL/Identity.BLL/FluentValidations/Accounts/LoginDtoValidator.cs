using FluentValidation;
using Identity.DTO.Accounts;

namespace Identity.BLL.FluentValidations.Accounts
{
    public sealed class LoginDtoValidator:AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(l => l.Email).NotEmpty().WithMessage("Email cannot be empty").EmailAddress().WithMessage("Email format is wrong");
            RuleFor(l => l.Password).NotEmpty().WithMessage("Password cannot be empty");


        }
    }
}
