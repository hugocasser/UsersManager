using Domain.Model;

namespace Application.Abstraction.Repositories;

public interface IRefreshTokensRepository
{
    public Task<IEnumerable<RefreshToken>> GetAllAsync(
        int? skipEntitiesCount, 
        int? takeEntitiesCount,
        CancellationToken cancellationToken);
    public Task AddAsync(RefreshToken token, CancellationToken cancellationToken);
    public RefreshToken Update(RefreshToken token);
    public Task<RefreshToken> FindUserTokenAsync(string userId, string token, CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}