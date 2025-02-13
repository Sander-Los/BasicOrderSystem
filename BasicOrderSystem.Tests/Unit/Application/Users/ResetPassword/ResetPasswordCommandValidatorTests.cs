using BasicOrderSystem.Application.Users.ResetPassword;
using FluentValidation.TestHelper;

namespace BasicOrderSystem.Tests.Unit.Application.Users.ResetPassword;

public class ResetPasswordCommandValidatorTests
{
    private readonly ResetPasswordCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var command = new ResetPasswordCommand("", "validToken", "NewPass123!", "NewPass123!");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var command = new ResetPasswordCommand("invalid-email", "validToken", "NewPass123!", "NewPass123!");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Should_Have_Error_When_ResetToken_Is_Empty()
    {
        var command = new ResetPasswordCommand("user@example.com", "", "NewPass123!", "NewPass123!");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.ResetToken);
    }

    [Fact]
    public void Should_Have_Error_When_NewPassword_Is_Empty()
    {
        var command = new ResetPasswordCommand("user@example.com", "validToken", "", "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Confirmation_Does_Not_Match()
    {
        var command = new ResetPasswordCommand("user@example.com", "validToken", "NewPass123!", "DifferentPass!");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.NewPasswordConfirmation);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new ResetPasswordCommand("user@example.com", "validToken", "NewPass123!", "NewPass123!");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
        result.ShouldNotHaveValidationErrorFor(c => c.ResetToken);
        result.ShouldNotHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.NewPasswordConfirmation);
    }
}
