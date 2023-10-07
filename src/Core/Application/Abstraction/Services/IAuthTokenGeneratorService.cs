namespace Application.Abstraction.Services;

public interface IAuthTokenGeneratorService
{
    public string GenerateToken(Guid userId, string userEmail, IEnumerable<string> roles);
}