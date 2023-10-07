using Domain.Model;

namespace Application.Abstraction.Services;

public interface IRefreshTokenGeneratorService
{
    public RefreshToken GenerateToken(Guid userId);
    public Task<RefreshToken> ValidateTokenAsync(Guid userId, string token, CancellationToken cancellationToken);
}