using MediatR;

namespace RO.DevTest.Application.Features.Customer.Commands.UpdateCustomerCommand;

public class UpdateCustomerWithIdCommand : IRequest<UpdateCustomerResult>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}