using Application.Abstraction.Repositories;
using Application.Dtos;
using Application.Exceptions;
using MediatR;

namespace Application.RequestHandlers.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ViewUserDto>
{
    private readonly IUsersRepository _usersRepository;

    public GetUserQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    public async Task<ViewUserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByIdAsync(request.Id);
        if (user is null)
        {
            throw new NotFoundExceptionWithStatusCode("user not found");
        }
        return ViewUserDto.MapFromModel(
            user, 
            await _usersRepository.GetUserRolesAsync(user));
    }
}