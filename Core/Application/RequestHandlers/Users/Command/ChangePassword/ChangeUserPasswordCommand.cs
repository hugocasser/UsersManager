using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.RequestHandlers.Users.Command.ChangePassword;

public record ChangeUserPasswordCommand(string OldPassword, string NewPassword, Guid Id) : IRequest<IdentityResult>;