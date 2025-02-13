using BasicOrderSystem.Application.Users.Register;
using FluentValidation;

namespace BasicOrderSystem.Application.Users.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.CurrentPassword).NotEmpty().WithMessage("Current password is required");
        RuleFor(c => c.NewPassword).NotEmpty().WithMessage("New password is required");
        RuleFor(c => c.ConfirmPassword)
            .NotEmpty().WithMessage("Please confirm the new password")
            .Equal(c => c.NewPassword).WithMessage("Passwords do not match");
    }
}