using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Domain.Model;
using MediatR;
using Application.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic.CompilerServices;
using Utils = Application.Common.Utils;

namespace Application.RequestHandlers.Auth.Commands;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IEmailConfirmMessageService _emailConfirmMessageService;

    public RegisterUserCommandHandler(IUsersRepository usersRepository,
        IEmailConfirmMessageService emailConfirmMessageService)
    {
        _usersRepository = usersRepository;
        _emailConfirmMessageService = emailConfirmMessageService;
    }

    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            PhoneNumber = request.Phone,
            FirstName = request.Name,
            UserName = request.UserName,
            Age = request.Age
        };

        var tempId = Guid.NewGuid();

        while (await _usersRepository.IsUserExistAsync(tempId))
        {
            tempId = Guid.NewGuid();
        }

        user.Id = tempId;

        var result = await _usersRepository.CreateUserAsync(user, request.Password);
        Utils.AggregateIdentityErrorsAndThrow(result);
        Utils.AggregateIdentityErrorsAndThrow(await _usersRepository.SetUserRoleAsync(user, UserRoles.User));
        await _emailConfirmMessageService.SendEmailConfirmMessageAsync(user);

        return result;
    }
}