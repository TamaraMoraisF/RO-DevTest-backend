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
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Name))
        {
            throw new BadRequestException("Name and Email are required.");
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