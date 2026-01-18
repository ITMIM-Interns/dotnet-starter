using FluentValidation;
using Identity.BLL.Helpers;
using Identity.DTO.Users;

namespace Identity.BLL.FluentValidations.Users
{
    public sealed class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(u => u.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(u => u.Username).MaximumLength(50).WithMessage("Username cannot exceed 50 characters").When(u=>u.Username is null);
            RuleFor(u => u.Image).Must(img => img is null || ImageService.IsImage(img)).WithMessage("Uploaded file must be a valid image (jpg, png)");
        }
    }
}
