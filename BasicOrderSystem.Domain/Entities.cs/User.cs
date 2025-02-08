using Microsoft.AspNetCore.Identity;

namespace BasicOrderSystem.Domain.Entities.cs;

public class User : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public bool IsActive { get; set; } = true;
}