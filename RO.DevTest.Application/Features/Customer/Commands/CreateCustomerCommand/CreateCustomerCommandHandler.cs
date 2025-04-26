using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using CustomerEntity = RO.DevTest.Domain.Entities.Customer;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Customer.Commands.CreateCustomerCommand;

public class CreateCustomerCommandHandler(ICustomerRepository customerRepo)
    : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
{
    private readonly ICustomerRepository _customerRepo = customerRepo;

    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {     
        var validator = new CreateCustomerCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult);
        }

        var customer = new CustomerEntity
        {
            Name = request.Name,
            Email = request.Email
        };

        await _customerRepo.CreateAsync(customer, cancellationToken);

        return new CreateCustomerResult(
            Id: customer.Id.ToString(),
            Name: customer.Name,
            Email: customer.Email
        );
    }
}