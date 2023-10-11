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

[Produces("application/json")]
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
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <remarks>create user and send verification mail</remarks>
    /// <returns>Nothing</returns>
    /// <response code ="200">Success</response>
    /// <response code ="403">Validation failed</response>
    /// <response code ="500">Some problems with smtp service or with db(you can see it in server response)</response>
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
    /// get user password and mail
    /// </remarks>
    /// <param name="command">
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <remarks>login user</remarks>
    /// <returns>Auth token</returns>
    /// <response code ="200">Success</response>
    /// <response code ="403">Validation failed</response>
    /// <response code ="401">Incorrect data</response>
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var token = await Sender.Send(command, cancellationToken);

        return Ok(token);
    }
    
    /// <summary>
    /// Use this method to confirm email
    /// </summary>
    /// <remarks>
    /// confirm user mail 
    /// </remarks>
    /// <param name="Id">
    /// guid
    /// </param>
    /// <param name="code">
    /// code from your mail
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>message with result</returns>
    /// <response code ="200">Success</response>
    /// <response code ="403">Validation failed</response>
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
    /// resend mail to user mail
    /// </remarks>
    /// <param name="command">
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>if success 200ok</returns>
    /// <response code ="200">Success</response>
    /// <response code ="403">Validation failed</response>
    /// <response code ="401">Incorrect data</response>
    [HttpPost]
    public async Task<IActionResult> ResendEmailVerificationToken(ResendEmailVerificationTokenCommand command, CancellationToken cancellationToken)
    {
        var token = await Sender.Send(command, cancellationToken);
        
        return Ok(token.Succeeded ? "Email verification token was send" : "Email verification token didnt send");
    }
    
    /// <summary>
    /// Use this method to refresh your jwt token
    /// </summary>
    /// <remarks>
    /// refresh
    /// </remarks>
    /// <param name="command">
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>new jwt token</returns>
    /// <response code ="200">Success</response>
    /// <response code ="403">Validation failed</response>
    [HttpPost("{userId}/{refreshToken}")]
    public async Task<IActionResult> RefreshToken(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(userId, refreshToken);
        var token = await Sender.Send(command, cancellationToken);
        
        return Ok(token);
    }
}