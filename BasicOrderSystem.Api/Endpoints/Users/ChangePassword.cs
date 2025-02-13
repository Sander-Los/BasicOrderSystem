using System.Security.Claims;
using BasicOrderSystem.Api.Extensions;
using BasicOrderSystem.Api.Infrastructure;
using BasicOrderSystem.Application.Users.ChangePassword;
using BasicOrderSystem.Application.Users.Login;
using MediatR;

namespace BasicOrderSystem.Api.Endpoints.Users;

public class ChangePassword : IEndpoint
{
    public sealed record Request(string CurrentPassword, string NewPassword, string ConfirmPassword);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/change-password",
                async (Request request, HttpContext httpContext, ISender sender, CancellationToken ct) =>
                {
                    var claims = httpContext.User.Claims.ToList();
                    var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Results.Unauthorized();
                    }

                    var command = new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword,
                        request.ConfirmPassword);

                    var result = await sender.Send(command, ct);

                    return result.Match(Results.Ok, CustomResults.Problem);
                })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}