using FluentValidation;
using MiniApp.BLL.Features.Commands.Users.Delete;

namespace MiniApp.BLL.FluentValidations.Users
{
    public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(u => u.Id).NotEmpty().WithMessage("Id cannot be empty");
        }
    }
}
