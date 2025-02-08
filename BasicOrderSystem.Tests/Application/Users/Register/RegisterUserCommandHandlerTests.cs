using BasicOrderSystem.Application.Users.Register;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Tests.TestCommon.Fakes;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;


namespace BasicOrderSystem.Tests.Application.Users.Register;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<Role>> _roleManagerMock;
    private readonly Mock<IPublisher> _publisherMock;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _userManagerMock = FakeUserManager.Create();
        _roleManagerMock = FakeRoleManager.Create();
        _publisherMock = new Mock<IPublisher>();
        _handler = new RegisterUserCommandHandler(_userManagerMock.Object, _roleManagerMock.Object,
            _publisherMock.Object);

        // Prepare a list of users as IQueryable<User>
        var users = new List<User>
            {
                new User
                {
                    Email = "existing@example.com",
                    FirstName = "Test firstName",
                    LastName = "Test lastName",
                }
            }
            .AsQueryable();

        var mockUsers = users.BuildMock();

        _userManagerMock.Setup(userManager => userManager.Users)
            .Returns(mockUsers);
        _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(userManager => userManager.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
    }

     [Fact]
    public async Task Should_Return_Failure_When_Email_Is_Not_Unique()
    {
        var command = new RegisterUserCommand("John", "Doe", "existing@example.com", "Password123", "User");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.EmailNotUnique);
    }

    [Fact]
    public async Task Should_Return_Failure_When_Role_Does_Not_Exist()
    {
        _roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

        var command = new RegisterUserCommand("John", "Doe", "newuser@example.com", "Password123", "InvalidRole");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.RoleNotFound("InvalidRole"));
    }

    [Fact]
    public async Task Should_Return_Failure_When_User_Creation_Fails()
    {
        _roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        var command = new RegisterUserCommand("John", "Doe", "newuser@example.com", "Password123", "User");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.RegistrationFailed);
    }

    [Fact]
    public async Task Should_Return_Success_When_User_Is_Created()
    {
        _roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

        var command = new RegisterUserCommand("John", "Doe", "newuser@example.com", "Password123", "User");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_Assign_User_To_Role()
    {
        _roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

        var command = new RegisterUserCommand("John", "Doe", "newuser@example.com", "Password123", "User");
        await _handler.Handle(command, CancellationToken.None);

        _userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Once);
    }

    [Fact]
    public async Task Should_Publish_UserRegisteredDomainEvent()
    {
        _roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

        var command = new RegisterUserCommand("John", "Doe", "newuser@example.com", "Password123", "User");
        var result = await _handler.Handle(command, CancellationToken.None);

        _publisherMock.Verify(p => p.Publish(
            It.Is<UserRegisteredDomainEvent>(e => e.UserId == result.Value),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}