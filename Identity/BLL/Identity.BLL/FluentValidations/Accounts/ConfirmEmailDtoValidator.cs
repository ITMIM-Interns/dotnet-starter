using FluentValidation;
using Identity.DTO.Accounts;

namespace Identity.BLL.FluentValidations.Accounts
{
    public sealed class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
    {
        public ConfirmEmailDtoValidator()
        {
            RuleFor(u => u.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(u=>u.Code).NotEmpty().WithMessage("Code is required");
        }
    }
}
