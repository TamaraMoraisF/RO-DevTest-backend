using FluentValidation;

namespace RO.DevTest.Application.Features.Customer.Commands.CreateCustomerCommand;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("The name field is required.");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("The email field is required.")
            .EmailAddress()
            .WithMessage("The email field must contain a valid email address.");
    }
}