using Application.Abstraction.Repositories;
using Domain.Model;
using MediatR;

namespace Application.RequestHandlers.Auth.Queries;

public class GetAllRefreshTokensQueryHandler : IRequestHandler<GetAllRefreshTokensQuery, IEnumerable<RefreshToken>>
{
    private readonly IRefreshTokensRepository _tokensRepository;

    public GetAllRefreshTokensQueryHandler(IRefreshTokensRepository tokensRepository)
    {
        _tokensRepository = tokensRepository;
    }

    public async Task<IEnumerable<RefreshToken>> Handle(GetAllRefreshTokensQuery request, CancellationToken cancellationToken)
    {
        return await _tokensRepository.GetAllAsync(10 * (request.Page - 1), request.Page, cancellationToken);
    }
}