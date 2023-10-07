using MediatR;

namespace Application.RequestHandlers.Users.Queries.GetUserRoles;

public record GetUserRolesQuery(Guid Id) : IRequest<IEnumerable<string>>;