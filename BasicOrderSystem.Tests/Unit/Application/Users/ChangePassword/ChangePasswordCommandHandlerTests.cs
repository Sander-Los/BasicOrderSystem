using BasicOrderSystem.Application.Abstractions.Authentication;
using BasicOrderSystem.Application.Users.ChangePassword;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Tests.Unit.TestCommon.Fakes;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BasicOrderSystem.Tests.Unit.Application.Users.ChangePassword
{
    public class ChangePasswordCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IPublisher> _publisherMock;
        private readonly Mock<ITokenProvider> _tokenProviderMock;
        private readonly ChangePasswordCommandHandler _handler;

        public ChangePasswordCommandHandlerTests()
        {
            _userManagerMock = FakeUserManager.Create();
            _publisherMock = new Mock<IPublisher>();
            _tokenProviderMock = new Mock<ITokenProvider>();

            _handler = new ChangePasswordCommandHandler(_userManagerMock.Object, _publisherMock.Object, _tokenProviderMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_User_Not_Found()
        {
            var command = new ChangePasswordCommand
            (
                UserId: "nonexistent",
                CurrentPassword: "oldPassword",
                NewPassword: "newPassword",
                ConfirmPassword: "newPassword"
            );

            _userManagerMock.Setup(u => u.FindByIdAsync(command.UserId))
                            .ReturnsAsync((User)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFound(command.UserId));
        }

        [Fact]
        public async Task Handle_Should_Return_Failure_When_Password_Change_Fails()
        {
            var user = new User { Id = "user1", Email = "test@example.com", FirstName = "test", LastName = "test lastName" };
            var command = new ChangePasswordCommand
            (
                UserId: user.Id,
                CurrentPassword: "wrongOldPassword",
                NewPassword: "newPassword",
                ConfirmPassword: "newPassword"
            );

            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid password" }));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.PasswordChangeFailed(user.Id));
        }

        [Fact]
        public async Task Handle_Should_Publish_Domain_Event_When_Email_Is_Not_Null_And_Return_New_Token()
        {
            var user = new User { Id = "user1", Email = "test@example.com", FirstName = "test", LastName = "test lastName" };
            var command = new ChangePasswordCommand
            (
                UserId: user.Id,
                CurrentPassword: "oldPassword",
                NewPassword: "newPassword",
                ConfirmPassword: "newPassword"
            );

            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword))
                            .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                            .ReturnsAsync(new List<string> { "User" });

            var newToken = "newToken123";
            _tokenProviderMock.Setup(tp => tp.Create(user, It.IsAny<IList<string>>()))
                              .Returns(newToken);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newToken);
            _publisherMock.Verify(p => p.Publish(
                It.Is<UserChangedPasswordDomainEvent>(e => e.Email == user.Email),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Not_Publish_Domain_Event_When_Email_Is_Null_But_Return_New_Token()
        {
            var user = new User { Id = "user1", Email = null, FirstName = "test", LastName = "test lastName" };
            var command = new ChangePasswordCommand
            (
                UserId: user.Id,
                CurrentPassword: "oldPassword",
                NewPassword: "newPassword",
                ConfirmPassword: "newPassword"
            );

            _userManagerMock.Setup(u => u.FindByIdAsync(user.Id))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword))
                            .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                            .ReturnsAsync(new List<string> { "User" });

            var newToken = "newToken123";
            _tokenProviderMock.Setup(tp => tp.Create(user, It.IsAny<IList<string>>()))
                              .Returns(newToken);
            
            var result = await _handler.Handle(command, CancellationToken.None);
            
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newToken);
            _publisherMock.Verify(p => p.Publish(It.IsAny<UserChangedPasswordDomainEvent>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
