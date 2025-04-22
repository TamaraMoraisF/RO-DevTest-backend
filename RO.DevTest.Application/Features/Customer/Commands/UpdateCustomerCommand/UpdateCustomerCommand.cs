using MediatR;

namespace RO.DevTest.Application.Features.Customer.Commands.UpdateCustomerCommand;

public class UpdateCustomerCommand : IRequest<UpdateCustomerResult>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}