using BasicOrderSystem.Application.Abstractions.Authentication;
using BasicOrderSystem.Application.Abstractions.Messaging;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Shared;
using Microsoft.AspNetCore.Identity;

namespace BasicOrderSystem.Application.Users.Login;

public class LoginUserCommandHandler(UserManager<User> userManager, ITokenProvider tokenProvider)
    : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Result.Failure<string>(UserErrors.NotFoundByEmail);
        }

        var verified = await userManager.CheckPasswordAsync(user, request.Password);

        if (!verified)
        {
            return Result.Failure<string>(UserErrors.NotFoundByEmail);
        }

        var userRoles = await userManager.GetRolesAsync(user);
        var token = tokenProvider.Create(user, userRoles);

        return token;
    }
}