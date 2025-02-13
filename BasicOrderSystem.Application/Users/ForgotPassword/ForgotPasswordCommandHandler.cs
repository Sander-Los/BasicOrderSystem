using BasicOrderSystem.Application.Abstractions.Messaging;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BasicOrderSystem.Application.Users.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(UserManager<User> userManager)
    : ICommandHandler<ForgotPasswordCommand, string>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user == null)
        {
            return Result.Failure<string>(UserErrors.NotFoundByEmail);
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
        // normally publish an event sending an email containing a link with the reset token inside. For demo purposes the token is returned
        return token;
    }
}