namespace Application.Dtos;

public record AuthTokens(Guid Id, string Token, string RefreshToken);