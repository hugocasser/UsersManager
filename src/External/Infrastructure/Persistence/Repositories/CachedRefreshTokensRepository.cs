using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Application.Common;
using Domain.Model;

namespace Infrastructure.Persistence.Repositories;

public class CachedRefreshTokensRepository : IRefreshTokensRepository
{
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly ICacheService _distributedCache;

    public CachedRefreshTokensRepository(IRefreshTokensRepository refreshTokensRepository, ICacheService distributedCache)
    {
        _refreshTokensRepository = refreshTokensRepository;
        _distributedCache = distributedCache;
    }

    public async Task<IEnumerable<RefreshToken>> GetAllAsync(
        int? skipEntitiesCount, 
        int? takeEntitiesCount,
        CancellationToken cancellationToken)
    {
        return await _distributedCache.GetOrCreateAsync(
                   CachingKeys.Tokens,
                   async () => await _refreshTokensRepository.GetAllAsync(skipEntitiesCount, takeEntitiesCount, cancellationToken)) ?? 
               await _refreshTokensRepository.GetAllAsync(skipEntitiesCount, takeEntitiesCount, cancellationToken);
    }

    public Task AddAsync(RefreshToken token, CancellationToken cancellationToken)
    {
        return _refreshTokensRepository.AddAsync(token, cancellationToken);
    }

    public RefreshToken Update(RefreshToken token)
    {
        return _refreshTokensRepository.Update(token);
    }

    public async Task<RefreshToken> FindUserTokenAsync(Guid userId, string token, CancellationToken cancellationToken)
    {
        return await _distributedCache.GetOrCreateAsync(
                   CachingKeys.UserToken(token, userId),
                   async () => await _refreshTokensRepository.FindUserTokenAsync(userId, token, cancellationToken)) ?? 
               await _refreshTokensRepository.FindUserTokenAsync(userId, token, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _refreshTokensRepository.SaveChangesAsync(cancellationToken);
    }
}