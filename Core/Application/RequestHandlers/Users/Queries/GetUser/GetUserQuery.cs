using Application.Dtos;
using MediatR;

namespace Application.RequestHandlers.Users.Queries.GetUser;

public record GetUserQuery(Guid Id) : IRequest<ViewUserDto>;
