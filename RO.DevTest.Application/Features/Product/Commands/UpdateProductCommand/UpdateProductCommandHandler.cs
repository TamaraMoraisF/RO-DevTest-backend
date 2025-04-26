using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Product.Handlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductWithIdCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductWithIdCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult);
        }

        var existing = _productRepository.Get(p => p.Id == request.Id)
                        ?? throw new NotFoundException(nameof(Product), request.Id);

        existing.Name = request.Name;
        existing.Price = request.Price;

        await _productRepository.Update(existing, cancellationToken);

        return new UpdateProductResult(
            Id: existing.Id,
            Name: existing.Name,
            Price: existing.Price
        );
    }
}