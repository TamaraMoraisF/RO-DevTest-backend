using MediatR;

namespace RO.DevTest.Application.Features.Customer.Commands.CreateCustomerCommand;

public class CreateCustomerCommand : IRequest<CreateCustomerResult>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}