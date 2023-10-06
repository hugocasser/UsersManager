using Domain.Model;
using MediatR;

namespace Application.RequestHandlers.Users.Command.GiveRoleToUser;

public record GiveRoleToUserCommand(Guid UserId, UserRoles Role) : IRequest<string>;