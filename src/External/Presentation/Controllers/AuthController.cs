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

[Route("api/[controller]/[action]")]
public sealed class AuthController : ApiController
{
    public AuthController(ISender sender) : base(sender)
    {
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok("The confirmation message was send to your email!");
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var token = await Sender.Send(command, cancellationToken);

        return Ok(token);
    }
    
    [HttpGet("{userId}/{code}")]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string code, CancellationToken cancellationToken)
    {
        var command = new ConfirmEmailCommand(userId, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)));
        var token = await Sender.Send(command, cancellationToken);
        
        return Ok(token.Succeeded ? "Thank you for confirming your mail." : "Your Email is not confirmed");
    }
    
    [HttpPost]
    public async Task<IActionResult> ResendEmailVerificationToken(ResendEmailVerificationTokenCommand command, CancellationToken cancellationToken)
    {
        var token = await Sender.Send(command, cancellationToken);
        
        return Ok(token.Succeeded ? "Email verification token was send" : "Email verification token didnt send");
    }

    [HttpPost("{userId}/{refreshToken}")]
    public async Task<IActionResult> RefreshToken(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(userId, refreshToken);
        var token = await Sender.Send(command, cancellationToken);
        
        return Ok(token);
    }
}