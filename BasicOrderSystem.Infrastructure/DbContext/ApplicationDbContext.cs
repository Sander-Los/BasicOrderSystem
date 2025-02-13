using BasicOrderSystem.Domain.Entities.cs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BasicOrderSystem.Infrastructure.DbContext;

public class ApplicationDbContext : IdentityDbContext<User, Role, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Role>().HasData(
            new Role { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new Role { Id = "2", Name = "SalesUser", NormalizedName = "SALES USER" },
            new Role { Id = "3", Name = "FinanceUser", NormalizedName = "FINANCE USER" },
            new Role { Id = "4", Name = "ShippingUser", NormalizedName = "SHIPPING USER" });
    }
}
