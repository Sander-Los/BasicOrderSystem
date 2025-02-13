using BasicOrderSystem.Api.Extensions;
using BasicOrderSystem.Api.Infrastructure;
using BasicOrderSystem.Application.Users.ForgotPassword;
using BasicOrderSystem.Application.Users.Login;
using MediatR;

namespace BasicOrderSystem.Api.Endpoints.Users;

public class ForgotPassword : IEndpoint
{
    public sealed record Request(string Email);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/forgot-password", async (Request request, ISender sender, CancellationToken ct) =>
        {
            var command = new ForgotPasswordCommand(request.Email);
            
            var result = await sender.Send(command, ct);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }   
}