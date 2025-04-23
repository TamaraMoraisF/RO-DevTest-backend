using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;
using RO.DevTest.Domain.Exception;
using ProductEntity = RO.DevTest.Domain.Entities.Product;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _productRepoMock = new();
        _handler = new CreateProductCommandHandler(_productRepoMock.Object);
    }

    [Fact(DisplayName = "Given valid data should create a product")]
    public async Task Handle_WhenValidData_ShouldCreateProduct()
    {
        var command = new CreateProductCommand
        {
            Name = "Notebook",
            Price = 3500.00M
        };

        _productRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<ProductEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductEntity p, CancellationToken _) => p);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Price.Should().Be(command.Price);
    }

    [Fact(DisplayName = "Given invalid data should throw BadRequestException")]
    public async Task Handle_WhenInvalidData_ShouldThrowBadRequestException()
    {
        var command = new CreateProductCommand
        {
            Name = "",
            Price = -10
        };

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>();
    }
}