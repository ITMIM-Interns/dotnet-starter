using FluentValidation;
using Identity.DTO.Accounts;

namespace Identity.BLL.FluentValidations.Accounts
{
    public sealed class ForgetPasswordDtoValidator : AbstractValidator<ForgetPasswordDto>
    {
        public ForgetPasswordDtoValidator()
        {
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required");
        }
    }
}
