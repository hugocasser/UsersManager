using FluentValidation;

namespace Application.RequestHandlers.Auth.Commands.LoginUser;

public class LoginUserCommandValidation : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidation()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter the correct email.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}