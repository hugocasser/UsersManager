using System.Text;
using Application.RequestHandlers.Auth.Commands;
using Application.RequestHandlers.Auth.Commands.ConfirmEmail;
using Application.RequestHandlers.Auth.Commands.LoginUser;
using Application.RequestHandlers.Auth.Commands.RefreshToken;
using Application.RequestHandlers.Auth.Commands.ResendEmailVerificationToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Presentation.Abstractions;

namespace Presentation.Controllers;

[Route("/health-check")]
public sealed class KuberController : ApiController
{
    public KuberController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<IActionResult> HealthCheck(CancellationToken cancellationToken)
    {
        return Ok();
    }
}