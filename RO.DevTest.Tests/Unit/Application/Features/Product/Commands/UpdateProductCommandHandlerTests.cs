using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;
using RO.DevTest.Application.Features.Product.Handlers;
using RO.DevTest.Domain.Exception;
using System.Linq.Expressions;
using ProductEntity = RO.DevTest.Domain.Entities.Product;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Commands;

public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly UpdateProductCommandHandler _handler;

    public UpdateProductCommandHandlerTests()
    {
        _mockRepo = new();
        _handler = new UpdateProductCommandHandler(_mockRepo.Object);
    }

    [Fact(DisplayName = "Given valid data should update a product")]
    public async Task Should_Update_Product_When_Data_Is_Valid()
    {
        var command = new UpdateProductWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Keyboard",
            Price = 99.99M
        };

        var existingProduct = new ProductEntity { Id = command.Id, Name = "Old", Price = 70M };

        _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                 .Returns(existingProduct);

        _mockRepo.Setup(r => r.Update(It.IsAny<ProductEntity>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Price.Should().Be(command.Price);
    }

    [Fact(DisplayName = "Given invalid data should throw BadRequestException")]
    public async Task Should_Throw_BadRequest_When_Validation_Fails()
    {
        var command = new UpdateProductWithIdCommand { Id = Guid.Empty, Name = "", Price = -50 };

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact(DisplayName = "Given product not found should throw NotFoundException")]
    public async Task Should_Throw_NotFound_When_Product_Not_Found()
    {
        var command = new UpdateProductWithIdCommand { Id = Guid.NewGuid(), Name = "Test", Price = 10 };

        _mockRepo.Setup(r => r.Get(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
                 .Returns((ProductEntity)null!);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
