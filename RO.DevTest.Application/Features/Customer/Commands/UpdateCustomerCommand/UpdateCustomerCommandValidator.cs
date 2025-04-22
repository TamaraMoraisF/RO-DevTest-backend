using FluentValidation;

namespace RO.DevTest.Application.Features.Customer.Commands.UpdateCustomerCommand;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerWithIdCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("The name field is required.");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("The email field is required.")
            .EmailAddress().WithMessage("The email field must be a valid email address.");
    }
}