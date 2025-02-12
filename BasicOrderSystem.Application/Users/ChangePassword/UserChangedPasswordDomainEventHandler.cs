using BasicOrderSystem.Domain.Users;
using MediatR;

namespace BasicOrderSystem.Application.Users.ChangePassword;

internal sealed class UserChangedPasswordDomainEventHandler : INotificationHandler<UserChangedPasswordDomainEvent>
{
    public Task Handle(UserChangedPasswordDomainEvent notification, CancellationToken cancellationToken)
    {
        // send email etc
        return Task.CompletedTask;
    }
}