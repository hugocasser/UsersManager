using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.RequestHandlers.Auth.Commands;

public record RegisterUserCommand(
    string Name,
    string Email,
    string Phone,
    string Password,
    string UserName,
    int Age) : IRequest<IdentityResult>;