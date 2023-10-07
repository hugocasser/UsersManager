using Application.Dtos;
using MediatR;

namespace Application.RequestHandlers.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(Guid UserId, string RefreshToken) : IRequest<AuthTokens>;