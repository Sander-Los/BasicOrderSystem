using BasicOrderSystem.Domain.Entities.cs;
using BasicOrderSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicOrderSystem.Infrastructure.Users;

internal sealed class UserRolesConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role { Id = "1", Name = RoleConstants.Administrator, NormalizedName = RoleConstants.NormalizedAdministrator },
            new Role { Id = "2", Name = RoleConstants.SalesUser, NormalizedName = RoleConstants.NormalizedSalesUser },
            new Role { Id = "3", Name = RoleConstants.FinanceUser, NormalizedName = RoleConstants.NormalizedFinanceUser },
            new Role { Id = "4", Name = RoleConstants.ShippingUser, NormalizedName = RoleConstants.NormalizedShippingUser });
    }
}