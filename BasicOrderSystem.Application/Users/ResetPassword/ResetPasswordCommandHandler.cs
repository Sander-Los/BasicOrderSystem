using BasicOrderSystem.Application.Abstractions.Authentication;
using BasicOrderSystem.Application.Abstractions.Messaging;
using BasicOrderSystem.Application.Users.Login;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Shared;
using Microsoft.AspNetCore.Identity;

namespace BasicOrderSystem.Application.Users.ResetPassword;

public class ResetPasswordCommandHandler(UserManager<User> userManager)
    : ICommandHandler<ResetPasswordCommand, string>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(command.Email);

        if (user is null)
        {
            return Result.Failure<string>(UserErrors.NotFoundByEmail);
        }

        var result = await userManager.ResetPasswordAsync(user, command.ResetToken, command.NewPassword);
        if (!result.Succeeded)
        {
            return Result.Failure<string>(UserErrors.PasswordChangeFailed(user.Id));
        }
        
        return "Password has been reset successfully";
    }
}