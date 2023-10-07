namespace Application.Abstraction.Services;

public interface ICacheService
{
    public Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T?>> factory);
}