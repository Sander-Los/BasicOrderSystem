using BasicOrderSystem.Application.Abstractions.Authentication;
using BasicOrderSystem.Application.Abstractions.Messaging;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BasicOrderSystem.Application.Users.ChangePassword;

public sealed class ChangePasswordCommandHandler(UserManager<User> userManager, IPublisher publisher, ITokenProvider tokenProvider)
    : ICommandHandler<ChangePasswordCommand, string>
{
    
    public async Task<Result<string>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId);
        if (user == null)
        {
            return Result.Failure<string>(UserErrors.NotFound(command.UserId));
        }
        var changeResult = await userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);

        if (!changeResult.Succeeded)
        {
            return Result.Failure<string>(UserErrors.PasswordChangeFailed(command.UserId));
        }

        if (user.Email != null)
        {
            await publisher.Publish(new UserChangedPasswordDomainEvent(user.Email), cancellationToken);
        }

        var userRoles = await userManager.GetRolesAsync(user);
        var token = tokenProvider.Create(user, userRoles);
        return token;

    }
}