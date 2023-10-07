using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Application.Common;
using Application.Exceptions;
using Domain.Model;

namespace Infrastructure.Services.TokenServices;

public class RefreshTokenGeneratorService : IRefreshTokenGeneratorService
{
    private readonly IRefreshTokensRepository _refreshTokensRepository;

    public RefreshTokenGeneratorService(IRefreshTokensRepository refreshTokensRepository)
    {
        _refreshTokensRepository = refreshTokensRepository;
    }

    public RefreshToken GenerateToken(Guid userId)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = Utils.GenerateRandomString(7),
            IsUsed = false,
            IsRevoked = false,
            AddedTime = DateTime.UtcNow,
            ExpiryTime = DateTime.UtcNow.AddMonths(2)
        };
    }

    public async Task<RefreshToken> ValidateTokenAsync(Guid userId, string token, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokensRepository.FindUserTokenAsync(userId, token, cancellationToken);
        if (refreshToken is null)
        {
            throw new IdentityExceptionWithStatusCode("The refresh token was not generated.");
        }
        
        if (refreshToken.IsRevoked || refreshToken.ExpiryTime < DateTime.UtcNow)
        {
            throw new IdentityExceptionWithStatusCode("The refresh token was expired or revoked. Please login again");
        }

        return refreshToken;
    }
}