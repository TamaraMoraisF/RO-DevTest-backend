using FluentValidation;

namespace RO.DevTest.Application.Features.Customer.Commands.DeleteCustomerCommand;

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O campo ID é obrigatório");
    }
}