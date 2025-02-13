using BasicOrderSystem.Application.Abstractions.Messaging;

namespace BasicOrderSystem.Application.Users.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : ICommand<string>;
