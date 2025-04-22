using MediatR;

namespace RO.DevTest.Application.Features.Customer.Commands.DeleteCustomerCommand;

public class DeleteCustomerCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
