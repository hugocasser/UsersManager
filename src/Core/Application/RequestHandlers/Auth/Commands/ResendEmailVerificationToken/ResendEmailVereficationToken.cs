using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.RequestHandlers.Auth.Commands.ResendEmailVerificationToken;

public class ResendEmailVerificationTokenCommandHandler : IRequestHandler<ResendEmailVerificationTokenCommand, IdentityResult>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IEmailConfirmMessageService _emailConfirmMessageService;

    public ResendEmailVerificationTokenCommandHandler(IUsersRepository usersRepository, IEmailConfirmMessageService emailConfirmMessageService)
    {
        _usersRepository = usersRepository;
        _emailConfirmMessageService = emailConfirmMessageService;
    }

    public async Task<IdentityResult> Handle(ResendEmailVerificationTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByEmailAsync(request.Email);
        if (user is null || !await _usersRepository.CheckPasswordAsync(user, request.Password)) 
        { 
            throw new IncorrectEmailOrPasswordExceptionWithStatusCode();
        }
        
        if (user.EmailConfirmed)
        {
            throw new IdentityExceptionWithStatusCode("The Email already confirmed");
        }
        
        await _emailConfirmMessageService.SendEmailConfirmMessageAsync(user);
        
        return IdentityResult.Success;
    }
}