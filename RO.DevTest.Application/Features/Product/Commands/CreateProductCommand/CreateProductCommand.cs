using MediatR;

namespace RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

public class CreateProductCommand : IRequest<CreateProductResult>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
