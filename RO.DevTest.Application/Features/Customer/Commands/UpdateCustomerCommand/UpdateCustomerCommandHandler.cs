using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Customer.Commands.UpdateCustomerCommand;

public class UpdateCustomerCommandHandler(ICustomerRepository customerRepo)
    : IRequestHandler<UpdateCustomerWithIdCommand, UpdateCustomerResult>
{
    private readonly ICustomerRepository _customerRepo = customerRepo;

    public async Task<UpdateCustomerResult> Handle(UpdateCustomerWithIdCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateCustomerCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult);
        }

        var existing = _customerRepo.Get(c => c.Id == request.Id) ?? throw new NotFoundException(nameof(Customer), request.Id);
        existing.Name = request.Name;
        existing.Email = request.Email;

        await _customerRepo.Update(existing, cancellationToken);

        return new UpdateCustomerResult(
            Id: existing.Id.ToString(),
            Name: existing.Name,
            Email: existing.Email
        );
    }
}