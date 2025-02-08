using BasicOrderSystem.Shared;

namespace BasicOrderSystem.Domain.Users;

public sealed record UserRegisteredDomainEvent(Guid UserId, string Email) : IDomainEvent;
