using Domain.Model;

namespace Application.Dtos;

public record ViewUserDto(
    Guid Id,
    string FirstName,
    string Email,
    string Phone,
    IEnumerable<string> Role)
{
    public static ViewUserDto MapFromModel(User model, IEnumerable<string> roles)
    {
        return new ViewUserDto(
            model.Id, 
            model.FirstName,
            model.Email,
            model.PhoneNumber,
            roles);
    }
}