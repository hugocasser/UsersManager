using FluentValidation;

namespace Application.RequestHandlers.Users.Queries.GetUserRoles;

public class GetUserRolesQueryValidation : AbstractValidator<GetUserRolesQuery>
{
    public GetUserRolesQueryValidation()
    {
        RuleFor(request => request.Id.ToString())
            .NotEmpty().WithMessage("User Id is required.")
            .Matches(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?");
    }
}