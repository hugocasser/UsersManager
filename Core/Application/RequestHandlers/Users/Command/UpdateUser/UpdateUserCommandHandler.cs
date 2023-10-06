using System.Security.Claims;
using Application.Abstraction.Repositories;
using Application.Exceptions;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.RequestHandlers.Users.Command.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateUserCommandHandler(IUsersRepository usersRepository, IHttpContextAccessor httpContextAccessor)
    {
        _usersRepository = usersRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userToUpdate = await _usersRepository.GetUserByIdAsync(request.Id);
        if (userToUpdate is null)
        {
            throw new NotFoundExceptionWithStatusCode("user not found");
        }
        var tempId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(tempId)) 
        {
            throw new InvalidOperationException("User claims was empty!");
        }
        
        var updatedUser = new  User
        {
            Id = request.Id
        };

        if (string.IsNullOrEmpty(request.Email))
        {
            updatedUser.Email = request.Email;
            updatedUser.EmailConfirmed = false;
        }
        if (string.IsNullOrEmpty(request.Phone))
        {
            updatedUser.PhoneNumber = request.Phone;
        }
        if (request.Age is not null)
        {
            updatedUser.Age = request.Age;
        }
        if (string.IsNullOrEmpty(request.Name))
        {
            updatedUser.FirstName = request.Name;
        }

        await _usersRepository.UpdateUserAsync(updatedUser);
    }
}