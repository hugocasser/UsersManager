using Application.Abstraction.Repositories;
using Application.Common;
using Application.Dtos;
using MediatR;

namespace Application.RequestHandlers.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<ViewUserDto>>
{
    private readonly IUsersRepository _usersRepository;

    public GetAllUsersQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<IEnumerable<ViewUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var usersQuery = _usersRepository
            .GetAllUsers()
            .OrderBy(x => x.UserName)
            .Skip(PageFetchSettings.ItemsOnPage * (request.Page - 1))
            .Take(PageFetchSettings.ItemsOnPage);

        var users = await _usersRepository.ToListAsync(usersQuery);
        
        var viewUserDtos = new List<ViewUserDto>();
        foreach (var user in users) 
        {
            viewUserDtos.Add(ViewUserDto.MapFromModel(user, await _usersRepository.GetUserRolesAsync(user)));
        }
        
        return viewUserDtos;
    }
}