using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.RequestHandlers.Users.Command.UpdateUser;

public class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
    {
        RuleFor(request => request.Id.ToString())
            .NotEmpty().WithMessage("User Id is required.")
            .Matches(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?")
            .WithMessage("The Guid ID is incorrect! Double-check your Guid ID.");
        RuleFor(request => request.Age)
            .GreaterThan(0).WithMessage("Age must be greater than zero");
        RuleFor(request => request.Email)
            .EmailAddress().WithMessage("Email address must be correct");
        RuleFor(request => request.Phone)
            .Matches(new Regex(@"^((8|\+374|\+994|\+995|\+375|\+7|\+380|\+38|\+996|\+998|\+993)[\- ]?)?\(?\d{3,5}\)?[\- ]?\d{1}[\- ]?\d{1}[\- ]?\d{1}[\- ]?\d{1}[\- ]?\d{1}(([\- ]?\d{1})?[\- ]?\d{1})?$")).WithMessage("PhoneNumber not valid");
    }
}