using Application.Dtos;
using MediatR;

namespace Application.RequestHandlers.Users.Queries.GetAllUsers;

public record GetAllUsersQuery(int Page) : IRequest<IEnumerable<ViewUserDto>>;