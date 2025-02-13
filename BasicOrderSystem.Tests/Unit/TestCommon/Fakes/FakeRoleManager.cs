using BasicOrderSystem.Domain.Entities.cs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace BasicOrderSystem.Tests.Unit.TestCommon.Fakes;

public static class FakeRoleManager
{
    public static Mock<RoleManager<Role>> Create()
    {
        return new Mock<RoleManager<Role>>(
            new Mock<IRoleStore<Role>>().Object,
            new IRoleValidator<Role>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<Role>>>().Object);
    }
}