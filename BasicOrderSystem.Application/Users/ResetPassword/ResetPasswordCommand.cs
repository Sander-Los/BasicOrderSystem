using BasicOrderSystem.Application.Abstractions.Messaging;

namespace BasicOrderSystem.Application.Users.ResetPassword;

public sealed record ResetPasswordCommand(string Email, string ResetToken, string NewPassword, string NewPasswordConfirmation) : ICommand<string>;
