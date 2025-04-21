using FluentValidation;

namespace RO.DevTest.Application.Features.Customer.Commands.CreateCustomerCommand;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("O campo nome é obrigatório");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("O campo e-mail é obrigatório")
            .EmailAddress()
            .WithMessage("O campo e-mail precisa ser um e-mail válido");
    }
}