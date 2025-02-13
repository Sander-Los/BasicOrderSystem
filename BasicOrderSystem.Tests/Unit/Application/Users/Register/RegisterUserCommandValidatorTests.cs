using BasicOrderSystem.Application.Users.Register;
using FluentValidation.TestHelper;

namespace BasicOrderSystem.Tests.Unit.Application.Users.Register;
public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var command = new RegisterUserCommand("", "Doe", "test@example.com", "Password123", "User");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        var command = new RegisterUserCommand("John", "", "test@example.com", "Password123", "User");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.LastName);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var command = new RegisterUserCommand("John", "Doe", "invalid-email", "Password123", "User");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var command = new RegisterUserCommand("John", "Doe", "test@example.com", "123", "User");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        var command = new RegisterUserCommand("John", "Doe", "test@example.com", "Password123", "User");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
