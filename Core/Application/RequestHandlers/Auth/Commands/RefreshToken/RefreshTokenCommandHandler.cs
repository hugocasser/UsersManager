using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Application.Dtos;
using Application.Exceptions;
using Application.RequestHandlers.Auth.Commands.RefreshToken;
using MediatR;

namespace Application.RequestHandlers.Auth.Commands.RefreshJwtToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthTokens>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IAuthTokenGeneratorService _authTokenGeneratorService;
    private readonly IRefreshTokenGeneratorService _refreshTokenGeneratorService;

    public RefreshTokenCommandHandler(
        IRefreshTokensRepository refreshTokensRepository, 
        IUsersRepository usersRepository, 
        IAuthTokenGeneratorService authTokenGeneratorService, 
        IRefreshTokenGeneratorService refreshTokenGeneratorService)
    {
        _refreshTokensRepository = refreshTokensRepository;
        _usersRepository = usersRepository;
        _authTokenGeneratorService = authTokenGeneratorService;
        _refreshTokenGeneratorService = refreshTokenGeneratorService;
    }

    public async Task<AuthTokens> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenGeneratorService.ValidateTokenAsync(request.UserId, request.RefreshToken, cancellationToken);
        var user = await _usersRepository.GetUserByIdAsync(refreshToken.UserId);
        if (user is null)
        {
            throw new NotFoundExceptionWithStatusCode("There is no user with this Id");
        }
        
        var userRoles = await _usersRepository.GetUserRolesAsync(user);
        var userToken = _authTokenGeneratorService.GenerateToken(user.Id, user.Email, userRoles);
        
        refreshToken.IsUsed = true;
        _refreshTokensRepository.Update(refreshToken);
        await _refreshTokensRepository.SaveChangesAsync(cancellationToken);

        return new AuthTokens(request.UserId, userToken, refreshToken.Token);
    }
}