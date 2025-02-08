using BasicOrderSystem.Shared;

namespace BasicOrderSystem.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with id: '{userId}' was not found.");

    public static Error UnAuthorized() => Error.Failure(
        "Users.Unauthorized",
        "You are not authorized to perform this action.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoudByEmail",
        "The user with the specified email was not found.");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email in not unique");

    public static readonly Error RegistrationFailed = Error.Failure(
        "Users.RegistrationFailed",
        "The registration of the user failed");

    public static Error RoleNotFound(string roleName) => Error.NotFound(
        "Users.RoleNotFound",
        $"The role '{roleName}' was not found.");
}