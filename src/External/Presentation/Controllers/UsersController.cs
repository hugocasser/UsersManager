using Application.Dtos;
using Application.RequestHandlers.Users.Command.ChangePassword;
using Application.RequestHandlers.Users.Command.GiveRoleToUser;
using Application.RequestHandlers.Users.Queries.GetAllUsers;
using Application.RequestHandlers.Users.Queries.GetUser;
using Application.RequestHandlers.Users.Queries.GetUserRoles;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Controllers;

[Produces("application/json")]
[Route("api/[controller]/")]
public sealed class UsersController : ApiController
{
    public UsersController(ISender sender) : base(sender)
    {
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>User</returns>
    /// <response code="200">Success</response>
    /// <response code="404">User not found</response>
    /// <response code="403">Validation failed</response>
    /// <response code="401">Unauthorized</response>
    [Authorize(Roles = "User, Admin, Support, SuperAdmin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var command = new GetUserQuery(id);
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }

    /// <summary>
    /// get user roles by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>list of user roles</returns>
    /// /// <response code="200">Success</response>
    /// <response code="404">User not found</response>
    /// <response code="403">Validation failed</response>
    /// <response code="401">Unauthorized</response>
    [Authorize(Roles = "Support, SuperAdmin")]
    [HttpGet("{id:Guid}/roles")]
    public async Task<IActionResult> GetUserRoles(Guid id, CancellationToken cancellationToken)
    {
        var command = new GetUserRolesQuery(id);
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// get all users
    /// </summary>
    /// <param name="page"></param>
    /// it needs for pagination
    /// <param name="cancellationToken"></param>
    /// <returns>list of users(count = page)</returns>
    /// <response code="200">Success</response>
    /// <response code="404">User not found</response>
    /// <response code="403">Validation failed</response>
    /// <response code="401">Unauthorized</response>
    [Authorize(Roles = "Admin")]
    [HttpGet("{page:int}")]
    public async Task<IActionResult> GetAllUsers(int page, CancellationToken cancellationToken)
    {
        var command = new GetAllUsersQuery(page);
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// use this method to change password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="changeUserPasswordDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Success</response>
    /// <response code="404">User not found</response>
    /// <response code="403">Validation failed</response>
    /// <response code="401">Unauthorized</response>
    [Authorize(Roles = "User, Admin, Support, SuperAdmin")]
    [HttpPut("{id:guid}/password")]
    public async Task<IActionResult> ChangeUserPassword(Guid id, ChangeUserPasswordDto changeUserPasswordDto, CancellationToken cancellationToken)
    {
        var command = new ChangeUserPasswordCommand(
            changeUserPasswordDto.OldPassword, 
            changeUserPasswordDto.NewPassword,
            id);
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    
    /// <summary>
    /// use this method to change user role
    /// </summary>
    /// <param name="id"></param>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>new jwt token</returns>
    /// <response code="200">Success</response>
    /// <response code="404">User not found</response>
    /// <response code="403">Validation failed</response>
    /// <response code="401">Unauthorized</response>
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("{id:guid}/set-role")]
    public async Task<IActionResult> SetRole(Guid id, Roles role, CancellationToken cancellationToken)
    {        
        var command = new GiveRoleToUserCommand(id, role);
        var token  = await Sender.Send(command, cancellationToken);
        
        return Ok(new { Token = token });
    }
}