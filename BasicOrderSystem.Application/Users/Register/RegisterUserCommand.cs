using BasicOrderSystem.Application.Abstractions.Messaging;

namespace BasicOrderSystem.Application.Users.Register;

public sealed record RegisterUserCommand(string FirstName, string LastName, string Email, string Password, string Role)
    : ICommand<Guid>;