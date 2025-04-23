using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Exception;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Features.Customer.Commands.DeleteCustomerCommand;

public class DeleteCustomerCommandHandler(ICustomerRepository customerRepo)
    : IRequestHandler<DeleteCustomerCommand, Unit>
{
    private readonly ICustomerRepository _customerRepo = customerRepo;

    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var existing = _customerRepo.Get(c => c.Id == request.Id) ?? throw new NotFoundException(nameof(Customer), request.Id);
        await _customerRepo.Delete(existing);
        return Unit.Value;
    }
}