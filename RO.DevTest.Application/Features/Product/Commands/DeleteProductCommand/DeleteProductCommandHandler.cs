using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;

public class DeleteProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var existing = _productRepository.Get(p => p.Id == request.Id)
                       ?? throw new NotFoundException(nameof(Product), request.Id);

        await _productRepository.Delete(existing, cancellationToken);
        return Unit.Value;
    }
}
