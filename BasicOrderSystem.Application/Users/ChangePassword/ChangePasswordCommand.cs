using BasicOrderSystem.Application.Abstractions.Messaging;

namespace BasicOrderSystem.Application.Users.ChangePassword;

public sealed record ChangePasswordCommand(
    string UserId, 
    string CurrentPassword, 
    string NewPassword, 
    string ConfirmPassword) : ICommand<string>;
