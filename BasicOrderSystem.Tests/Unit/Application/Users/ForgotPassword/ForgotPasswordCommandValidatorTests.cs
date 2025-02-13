using BasicOrderSystem.Application.Users.ForgotPassword;
using FluentValidation.TestHelper;

namespace BasicOrderSystem.Tests.Unit.Application.Users.ForgotPassword
{
    public class ForgotPasswordCommandValidatorTests
    {
        private readonly ForgotPasswordCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            var command = new ForgotPasswordCommand("");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var command = new ForgotPasswordCommand("invalid-email");
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Email_Is_Valid()
        {
            var command = new ForgotPasswordCommand("valid@example.com");
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(c => c.Email);
        }
    }
}