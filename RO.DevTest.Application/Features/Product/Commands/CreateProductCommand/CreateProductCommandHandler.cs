using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using ProductEntity = RO.DevTest.Domain.Entities.Product;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

public class CreateProductCommandHandler(IProductRepository productRepo)
    : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepo = productRepo;

    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult);
        }

        var product = new ProductEntity
        {
            Name = request.Name,
            Price = request.Price
        };

        await _productRepo.CreateAsync(product, cancellationToken);

        return new CreateProductResult(
            Id: product.Id.ToString(),
            Name: product.Name,
            Price: product.Price
        );
    }
}
