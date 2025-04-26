using MediatR;

namespace RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;

public class DeleteProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
