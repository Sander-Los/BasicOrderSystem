using BasicOrderSystem.Api.Extensions;
using BasicOrderSystem.Api.Infrastructure;
using BasicOrderSystem.Application.Users.Register;
using BasicOrderSystem.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicOrderSystem.Api.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public sealed record Request(string Email, string FirstName, string LastName, string Password, string Role);
    
    [AllowAnonymous]
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async ([FromBody] Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new RegisterUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password,
                    request.Role);

                Result<Guid> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Users);
    }
}