using BasicOrderSystem.Application.Abstractions.Messaging;
using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Domain.Users;
using BasicOrderSystem.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicOrderSystem.Application.Users.Register;

public sealed class RegisterUserCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager, IPublisher publisher)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await userManager.Users.AnyAsync(user => user.Email == command.Email, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }
       
        if (!(await roleManager.RoleExistsAsync(command.Role)))
        {
            return Result.Failure<Guid>(UserErrors.RoleNotFound(command.Role));
        }

        var user = new User
        {
            UserName = command.Email,
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName
        };
        var result = await userManager.CreateAsync(user, command.Password);
        
        if (!result.Succeeded)
        {
            return Result.Failure<Guid>(UserErrors.RegistrationFailed);
        }
        await userManager.AddToRoleAsync(user, command.Role);

        var userId = Guid.Parse(user.Id);
        await publisher.Publish(new UserRegisteredDomainEvent(userId, command.Email), cancellationToken);

        return userId;

    }
}