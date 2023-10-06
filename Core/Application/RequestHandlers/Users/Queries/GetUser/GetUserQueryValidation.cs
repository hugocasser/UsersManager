using FluentValidation;

namespace Application.RequestHandlers.Users.Queries.GetUser;

public class GetUserQueryValidation : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidation()
    {
        RuleFor(request => request.Id.ToString())
            .NotEmpty().WithMessage("User Id is required.")
            .Matches(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?")
            .WithMessage("The Guid ID is incorrect! Double-check your Guid ID.");
    }
}