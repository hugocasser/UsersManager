using System.Security.Claims;
using Application.Abstraction.Repositories;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.RequestHandlers.Users.Command.ChangePassword;

public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, IdentityResult>
{
    private readonly IUsersRepository _versityUsersRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChangeUserPasswordCommandHandler(IUsersRepository usersRepository, IHttpContextAccessor httpContextAccessor)
    {
        _versityUsersRepository = usersRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IdentityResult> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = request.Id;
        var tempId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(tempId)) 
        {
            throw new InvalidOperationException("User claims was empty!");
        }

        var user = await _versityUsersRepository.GetUserByIdAsync(userId);
        var claimId = Guid.Parse(tempId);
        var claimUser = await _versityUsersRepository.GetUserByIdAsync(claimId);
        if (user is null || claimUser is null) 
        {
            throw new NotFoundExceptionWithStatusCode("There is no user with this Id");
        }
        if (!await _versityUsersRepository.CheckPasswordAsync(user, request.OldPassword))
        {
            throw new IncorrectEmailOrPasswordExceptionWithStatusCode();
        }
        
        if (userId != claimId)
        {
            var userRoles = await _versityUsersRepository.GetUserRolesAsync(claimUser);
            if (!userRoles.Contains("SuperAdmin" ))
            {
                throw new ExceptionWithStatusCode(StatusCodes.Status403Forbidden, "Not enough rights");
            }
        }
        
        var token = await _versityUsersRepository.GeneratePasswordResetTokenAsync(user);
        
        return await _versityUsersRepository.ResetPasswordAsync(user, token, request.NewPassword);
    }
}