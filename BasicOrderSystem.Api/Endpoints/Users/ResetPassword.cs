using BasicOrderSystem.Api.Extensions;
using BasicOrderSystem.Api.Infrastructure;
using BasicOrderSystem.Application.Users.ForgotPassword;
using BasicOrderSystem.Application.Users.Login;
using BasicOrderSystem.Application.Users.ResetPassword;
using MediatR;

namespace BasicOrderSystem.Api.Endpoints.Users;

public class ResetPassword : IEndpoint
{
    public sealed record Request(string Email, string ResetToken, string NewPassword, string NewPasswordConfirmation);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/reset-password", async (Request request, ISender sender, CancellationToken ct) =>
        {
            var command = new ResetPasswordCommand(request.Email, request.ResetToken, request.NewPassword, request.NewPasswordConfirmation);
            
            var result = await sender.Send(command, ct);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }   
}