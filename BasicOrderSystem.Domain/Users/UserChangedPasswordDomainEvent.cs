using BasicOrderSystem.Shared;

namespace BasicOrderSystem.Domain.Users;

public sealed record UserChangedPasswordDomainEvent(string Email) : IDomainEvent;
