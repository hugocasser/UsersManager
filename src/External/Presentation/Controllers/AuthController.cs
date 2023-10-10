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
    
    /// <summary>
    /// Use this method to register
    /// </summary>
    /// <remarks>
    /// Register user, and send verification mail
    /// </remarks>
    /// <param name="command">
    /// gets name,email, phone, password, username, age and send confirm mail
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>if success 200ok</returns>
    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok("The confirmation message was send to your email!");
    }
    
    /// <summary>
    /// Use this method to login
    /// </summary>
    /// <remarks>
    /// login user
    /// </remarks>
    /// <param name="command">
    /// email and password
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>if success 200ok and jwt token</returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var token = await Sender.Send(command, cancellationToken);

        return Ok(token);
    }
    
    /// <summary>
    /// Use this method to verify email
    /// </summary>
    /// <remarks>
    /// confirm
    /// </remarks>
    /// <param name="command">
    /// user id and verification code
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>if success 200ok</returns>
    [HttpGet("{userId}/{code}")]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string code, CancellationToken cancellationToken)
    {
        var command = new ConfirmEmailCommand(userId, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)));
        var token = await Sender.Send(command, cancellationToken);
        
        return Ok(token.Succeeded ? "Thank you for confirming your mail." : "Your Email is not confirmed");
    }
    
    /// <summary>
    /// Use this method if you didn't receive mail
    /// </summary>
    /// <remarks>
    /// resend
    /// </remarks>
    /// <param name="command">
    /// user email and password
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>if success 200ok</returns>
    [HttpPost]
    public async Task<IActionResult> ResendEmailVerificationToken(ResendEmailVerificationTokenCommand command, CancellationToken cancellationToken)
    {
        var token = await Sender.Send(command, cancellationToken);
        
        return Ok(token.Succeeded ? "Email verification token was send" : "Email verification token didnt send");
    }
    
    /// <summary>
    /// Use this method if you didn't receive mail
    /// </summary>
    /// <remarks>
    /// refresh
    /// </remarks>
    /// <param name="command">
    /// user id and refreshToken
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>if success 200ok and new jwt token</returns>
    [HttpPost("{userId}/{refreshToken}")]
    public async Task<IActionResult> RefreshToken(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(userId, refreshToken);
        var token = await Sender.Send(command, cancellationToken);
        
        return Ok(token);
    }
}