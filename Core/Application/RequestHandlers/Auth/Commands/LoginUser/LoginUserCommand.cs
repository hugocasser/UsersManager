using Application.Dtos;
using MediatR;

namespace Application.RequestHandlers.Auth.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<AuthTokens>;