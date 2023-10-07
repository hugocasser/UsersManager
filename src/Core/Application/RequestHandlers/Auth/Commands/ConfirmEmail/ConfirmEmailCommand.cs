using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.RequestHandlers.Auth.Commands.ConfirmEmail;

public record ConfirmEmailCommand(Guid UserId, string Token) : IRequest<IdentityResult>;