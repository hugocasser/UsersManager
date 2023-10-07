using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Application.Dtos;
using Application.Exceptions;
using Domain.Model;
using MediatR;

namespace Application.RequestHandlers.Auth.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthTokens>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IAuthTokenGeneratorService _authTokenGeneratorService;
    private readonly IRefreshTokenGeneratorService _refreshTokenGeneratorService;

    public LoginUserCommandHandler(
        IAuthTokenGeneratorService authTokenGeneratorService, 
        IUsersRepository usersRepository, 
        IRefreshTokensRepository refreshTokensRepository, 
        IRefreshTokenGeneratorService refreshTokenGeneratorService)
    {
        _authTokenGeneratorService = authTokenGeneratorService;
        _usersRepository = usersRepository;
        _refreshTokensRepository = refreshTokensRepository;
        _refreshTokenGeneratorService = refreshTokenGeneratorService;
    }

    public async Task<AuthTokens> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserByEmail(request);
        var userRoles = await _usersRepository.GetUserRolesAsync(user);
        
        var userToken = _authTokenGeneratorService.GenerateToken(user.Id, user.Email, userRoles);
        var refreshToken = _refreshTokenGeneratorService.GenerateToken(user.Id);
        
        await _refreshTokensRepository.AddAsync(refreshToken, cancellationToken);
        await _refreshTokensRepository.SaveChangesAsync(cancellationToken);

        return new AuthTokens(user.Id, userToken, refreshToken.Token);
    }

    private async Task<User?> GetUserByEmail(LoginUserCommand request)
    {
        var user = await _usersRepository.GetUserByEmailAsync(request.Email);
        
        if (user is null || !await _usersRepository.CheckPasswordAsync(user, request.Password))
        {
            throw new IncorrectEmailOrPasswordExceptionWithStatusCode();
        }
        
        if (!user.EmailConfirmed)
        {
            throw new EmailNotConfirmedExceptionWithStatusCode();
        }

        return user;
    }
}