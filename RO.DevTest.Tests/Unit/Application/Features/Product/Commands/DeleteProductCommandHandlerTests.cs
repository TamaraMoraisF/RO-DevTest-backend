using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;
using RO.DevTest.Domain.Exception;
using System.Linq.Expressions;
using ProductEntity = RO.DevTest.Domain.Entities.Product;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Commands;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly DeleteProductCommandHandler _handler;

    public DeleteProductCommandHandlerTests()
    {
        _repoMock = new();
        _handler = new DeleteProductCommandHandler(_repoMock.Object);
    }

    [Fact(DisplayName = "Given valid ID should delete the product")]
    public async Task Should_Delete_Product_When_Id_Is_Valid()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new ProductEntity { Id = productId, Name = "Monitor", Price = 699.99M };

        _repoMock.Setup(r => r.Get(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                 .Returns(existingProduct);

        _repoMock.Setup(r => r.Delete(existingProduct, It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        var command = new DeleteProductCommand { Id = productId };

        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.Delete(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Given non-existent ID should throw NotFoundException")]
    public async Task Should_Throw_NotFound_When_Product_Does_Not_Exist()
    {
        var command = new DeleteProductCommand { Id = Guid.NewGuid() };

        _repoMock.Setup(r => r.Get(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                 .Returns((ProductEntity)null!);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
