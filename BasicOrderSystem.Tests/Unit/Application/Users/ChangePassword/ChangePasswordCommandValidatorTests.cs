using BasicOrderSystem.Application.Users.ChangePassword;
using FluentValidation.TestHelper;

namespace BasicOrderSystem.Tests.Unit.Application.Users.ChangePassword
{
    public class ChangePasswordCommandValidatorTests
    {
        private readonly ChangePasswordCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_CurrenPassword_Is_Empty()
        {
            var command = new ChangePasswordCommand
            (
                UserId: string.Empty,
                CurrentPassword :"",
                NewPassword :"newPassword123",
                ConfirmPassword : "newPassword123"
            );
            
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.CurrentPassword)
                  .WithErrorMessage("Current password is required");
        }

        [Fact]
        public void Should_Have_Error_When_NewPassword_Is_Empty()
        {
            var command = new ChangePasswordCommand
            (
                UserId: string.Empty,
                CurrentPassword :"oldPassword123",
                NewPassword :"",
                ConfirmPassword : ""
            );

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.NewPassword)
                  .WithErrorMessage("New password is required");
            result.ShouldHaveValidationErrorFor(c => c.ConfirmPassword)
                  .WithErrorMessage("Please confirm the new password");
        }

        [Fact]
        public void Should_Have_Error_When_ConfirmPassword_Does_Not_Match_NewPassword()
        {
            var command = new ChangePasswordCommand
            (
                UserId: string.Empty,
                CurrentPassword :"oldPassword123",
                NewPassword :"newPassword123",
                ConfirmPassword : "differentPassword"
            );

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.ConfirmPassword)
                  .WithErrorMessage("Passwords do not match");
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            var command = new ChangePasswordCommand
            (
                UserId: string.Empty,
                CurrentPassword :"oldPassword123",
                NewPassword :"newPassword123",
                ConfirmPassword : "newPassword123"
            );

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
