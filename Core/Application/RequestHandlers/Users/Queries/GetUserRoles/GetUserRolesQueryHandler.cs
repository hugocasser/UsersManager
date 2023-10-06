using Application.Abstraction.Repositories;
using Application.Exceptions;
using Application.RequestHandlers.Users.Queries.GetUser;
using MediatR;

namespace Application.RequestHandlers.Users.Queries.GetUserRoles;

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IEnumerable<string>>
{
    private readonly IUsersRepository _usersRepository;

    public GetUserRolesQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<IEnumerable<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByIdAsync(request.Id);
        if (user is null) 
        {
            throw new NotFoundExceptionWithStatusCode("There is no user with this Id");
        }
        
        return await _usersRepository.GetUserRolesAsync(user);
    }
}