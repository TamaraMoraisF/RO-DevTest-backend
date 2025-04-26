using MediatR;

namespace RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;

public class UpdateProductWithIdCommand : IRequest<UpdateProductResult>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
