namespace Application.Common;

public static class CachingKeys
{
    public const string Tokens = "tokens";
    public static string UserToken(string token, Guid userId) => $"token-{token.ToString()}-user-{userId.ToString()}";
}