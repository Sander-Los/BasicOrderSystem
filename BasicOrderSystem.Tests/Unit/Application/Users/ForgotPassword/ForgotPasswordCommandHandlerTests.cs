using BasicOrderSystem.Application.Users.ForgotPassword;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Tests.Unit.TestCommon.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BasicOrderSystem.Tests.Unit.Application.Users.ForgotPassword
{
    public class ForgotPasswordCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly ForgotPasswordCommandHandler _handler;

        public ForgotPasswordCommandHandlerTests()
        {
            _userManagerMock = FakeUserManager.Create();

            _handler = new ForgotPasswordCommandHandler(_userManagerMock.Object);
        }

        [Fact]
        public async Task Should_Return_Failure_When_User_Not_Found()
        {
            // Arrange
            var command = new ForgotPasswordCommand("nonexistent@example.com");
            _userManagerMock.Setup(m => m.FindByEmailAsync(command.Email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFoundByEmail);
        }

        [Fact]
        public async Task Should_Return_Token_When_User_Exists()
        {
            // Arrange
            var user = new User { Email = "existing@example.com", FirstName = "Test", LastName = "User" };
            var command = new ForgotPasswordCommand(user.Email);
            var expectedToken = "reset-token-123";

            _userManagerMock.Setup(m => m.FindByEmailAsync(command.Email))
                .ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(expectedToken);
        }
    }
}
