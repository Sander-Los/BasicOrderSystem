using BasicOrderSystem.Domain.Entities.cs;

namespace BasicOrderSystem.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(User user, IList<string> userRoles);
}