using BasicOrderSystem.Application.Abstractions.Messaging;

namespace BasicOrderSystem.Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<string>;
