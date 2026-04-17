using FluentValidation;

namespace FinanceTracker.Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
    }
}
