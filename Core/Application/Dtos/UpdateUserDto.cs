namespace Application.Dtos;

public record UpdateUserDto(string? Name, int? Age, string? Email, string? Phone, Guid Id);