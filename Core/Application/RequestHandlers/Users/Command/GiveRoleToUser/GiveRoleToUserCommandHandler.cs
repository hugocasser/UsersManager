using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Application.Exceptions;
using MediatR;

namespace Application.RequestHandlers.Users.Command.GiveRoleToUser;

public class GiveRoleToUserCommandHandler: IRequestHandler<GiveRoleToUserCommand, string>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IAuthTokenGeneratorService _tokenGeneratorService;

    public GiveRoleToUserCommandHandler(IUsersRepository usersRepository, IAuthTokenGeneratorService tokenGeneratorService)
    {
        _usersRepository = usersRepository;
        _tokenGeneratorService = tokenGeneratorService;
    }

    public async Task<string> Handle(GiveRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByIdAsync(request.UserId);
        if (user is null)
        {
            throw new NotFoundExceptionWithStatusCode("There is no user with this Id");
        }
        
        await _usersRepository.SetUserRoleAsync(user, request.Role);
        var userRoles = await _usersRepository.GetUserRolesAsync(user);
        
        return _tokenGeneratorService.GenerateToken(user.Id, user.NormalizedEmail, userRoles);
    }
}