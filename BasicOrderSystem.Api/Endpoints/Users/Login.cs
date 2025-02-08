using BasicOrderSystem.Api.Extensions;
using BasicOrderSystem.Api.Infrastructure;
using BasicOrderSystem.Application.Users.Login;
using MediatR;

namespace BasicOrderSystem.Api.Endpoints.Users;

public class Login : IEndpoint
{
    public sealed record Request(string Email, string Password);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", async (Request request, ISender senden, CancellationToken ct) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            
            var result = await senden.Send(command, ct);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }   
}