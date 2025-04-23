using MediatR;

namespace RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;

public class UpdateProductCommand : IRequest<UpdateProductResult>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}