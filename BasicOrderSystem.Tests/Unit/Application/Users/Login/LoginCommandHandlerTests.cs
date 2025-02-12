using BasicOrderSystem.Application.Abstractions.Authentication;
using BasicOrderSystem.Application.Users.Login;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Tests.Unit.TestCommon.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BasicOrderSystem.Tests.Unit.Application.Users.Login;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenProvider> _tokenProviderMock;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _userManagerMock = FakeUserManager.Create();
        _tokenProviderMock = new Mock<ITokenProvider>();
        _handler = new LoginUserCommandHandler(_userManagerMock.Object, _tokenProviderMock.Object);
    }

    [Fact]
    public async Task Should_Return_Failure_When_User_Not_Found()
    {
        var command = new LoginUserCommand("nonexistent@example.com", "Password123");
        _userManagerMock
            .Setup(m => m.FindByNameAsync(command.Email))
            .ReturnsAsync((User)null);
       
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.NotFoundByEmail);
    }

    [Fact]
    public async Task Should_Return_Failure_When_Password_Is_Invalid()
    {
        var command = new LoginUserCommand("user@example.com", "Password123");
        var user = new User
        {
            Email = command.Email,
            FirstName = "Test",
            LastName = "User"
        };

        _userManagerMock
            .Setup(m => m.FindByNameAsync(command.Email))
            .ReturnsAsync(user);
        _userManagerMock
            .Setup(m => m.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(false); 

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.NotFoundByEmail);
    }

    [Fact]
    public async Task Should_Return_Success_With_Token_When_Credentials_Are_Valid()
    {
        var command = new LoginUserCommand("user@example.com", "Password123");
        var user = new User
        {
            Email = command.Email,
            FirstName = "Test",
            LastName = "User",
            Id = "aaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa"
        };
        var roles = new List<string> { "User" };
        const string expectedToken = "valid_token";

        _userManagerMock
            .Setup(m => m.FindByNameAsync(command.Email))
            .ReturnsAsync(user);
        _userManagerMock
            .Setup(m => m.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);
        _userManagerMock
            .Setup(m => m.GetRolesAsync(user))
            .ReturnsAsync(roles);
        _tokenProviderMock
            .Setup(tp => tp.Create(user, roles))
            .Returns(expectedToken);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expectedToken);
    }
}