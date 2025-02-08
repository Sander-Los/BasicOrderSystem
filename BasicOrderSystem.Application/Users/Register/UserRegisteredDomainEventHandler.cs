using BasicOrderSystem.Domain.Users;
using MediatR;

namespace BasicOrderSystem.Application.Users.Register;

internal sealed class UserRegisteredDomainEventHandler()
    : INotificationHandler<UserRegisteredDomainEvent>
{
    
    public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        // send email etc
        return Task.CompletedTask;
    }
}