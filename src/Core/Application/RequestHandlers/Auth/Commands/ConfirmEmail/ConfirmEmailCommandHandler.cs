using Application.Abstraction.Repositories;
using Application.Common;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.RequestHandlers.Auth.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, IdentityResult>
{
    private readonly IUsersRepository _usersRepository;

    public ConfirmEmailCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<IdentityResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByIdAsync(request.UserId);
        if (user is null)
        {
            throw new NotFoundExceptionWithStatusCode("There is no user with this Id");
        }
        
        var result = await _usersRepository.ConfirmUserEmail(user, request.Token);
        Utils.AggregateIdentityErrorsAndThrow(result);

        return result;
    }
}