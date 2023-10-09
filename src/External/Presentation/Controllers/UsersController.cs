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
using StackExchange.Redis;

namespace Presentation.Controllers;

[Route("api/[controller]/")]
public sealed class UsersController : ApiController
{
    public UsersController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var command = new GetUserQuery(id);
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }

    [Authorize(Roles = "Support, SuperAdmin")]
    [HttpGet("{id:Guid}/roles")]
    public async Task<IActionResult> GetUserRoles(Guid id, CancellationToken cancellationToken)
    {
        var command = new GetUserRolesQuery(id);
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{page:int}")]
    public async Task<IActionResult> GetAllUsers(int page, CancellationToken cancellationToken)
    {
        var command = new GetAllUsersQuery(page);
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
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
    
    
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("{id:guid}/set-admin")]
    public async Task<IActionResult> SetAdmin(Guid id, Roles role, CancellationToken cancellationToken)
    {        
        var command = new GiveRoleToUserCommand(id, role);
        var token  = await Sender.Send(command, cancellationToken);
        
        return Ok(new { Token = token });
    }
}