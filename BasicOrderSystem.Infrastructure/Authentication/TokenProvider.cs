using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BasicOrderSystem.Application.Abstractions.Authentication;
using BasicOrderSystem.Domain.Entities.cs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BasicOrderSystem.Infrastructure.Authentication;

internal sealed class TokenProvider(IConfiguration configuration) : ITokenProvider
{
    public string Create(User user, IList<string> userRoles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var claims = new[]
        { 
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        foreach (var role in userRoles)
        {
            claims = claims.Append(new Claim(ClaimTypes.Role, role)).ToArray();
        }

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}