using BasicOrderSystem.Application.Users.ResetPassword;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Tests.Unit.TestCommon.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BasicOrderSystem.Tests.Unit.Application.Users.ResetPassword;

public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly ResetPasswordCommandHandler _handler;

    public ResetPasswordCommandHandlerTests()
    {
        _userManagerMock = FakeUserManager.Create();
        _handler = new ResetPasswordCommandHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task Should_Return_Failure_When_User_Not_Found()
    {
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync((User)null!); // Simulate user not found

        var command = new ResetPasswordCommand("nonexistent@example.com", "validToken", "NewPass123!", "NewPass123!");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.NotFoundByEmail);
    }

    [Fact]
    public async Task Should_Return_Failure_When_Reset_Fails()
    {
        var user = new User { Id = Guid.NewGuid().ToString(), Email = "user@example.com", FirstName = "test", LastName = "user" };
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync(user);
        _userManagerMock.Setup(m => m.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Reset failed" }));

        var command = new ResetPasswordCommand("user@example.com", "invalidToken", "NewPass123!", "NewPass123!");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.PasswordChangeFailed(user.Id));
    }

    [Fact]
    public async Task Should_Return_Success_When_Password_Is_Reset()
    {
        var user = new User { Id = Guid.NewGuid().ToString(), Email = "user@example.com", FirstName = "test", LastName = "user" };
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync(user);
        _userManagerMock.Setup(m => m.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

        var command = new ResetPasswordCommand("user@example.com", "validToken", "NewPass123!", "NewPass123!");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Password has been reset successfully");
    }
}
