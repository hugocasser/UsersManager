using MediatR;

namespace Application.RequestHandlers.Users.Command.UpdateUser;

public record UpdateUserCommand(
    string? Name,
    int? Age,
    string? Email,
    string? Phone,
    Guid Id) : IRequest;