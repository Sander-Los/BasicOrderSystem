using FluentValidation;

namespace BasicOrderSystem.Application.Users.ResetPassword;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("Provide correct email address.");
        RuleFor(c => c.ResetToken).NotEmpty().WithMessage("Provide correct reset token.");
        RuleFor(c => c.NewPassword).NotEmpty().WithMessage("Provide correct new password.");
        RuleFor(c => c.NewPasswordConfirmation)
            .NotEmpty().WithMessage("Please confirm the new password")
            .Equal(c => c.NewPassword).WithMessage("Passwords do not match");
       
    }
}