using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;
using RO.DevTest.Domain.Exception;
using System.Linq.Expressions;
using CustomerEntity = RO.DevTest.Domain.Entities.Customer;
using ProductEntity = RO.DevTest.Domain.Entities.Product;
using SaleEntity = RO.DevTest.Domain.Entities.Sale;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Commands;

public class CreateSaleCommandHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepoMock;
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly CreateSaleCommandHandler _handler;

    public CreateSaleCommandHandlerTests()
    {
        _saleRepoMock = new();
        _customerRepoMock = new();
        _productRepoMock = new();
        _handler = new CreateSaleCommandHandler(
            _saleRepoMock.Object,
            _customerRepoMock.Object,
            _productRepoMock.Object
        );
    }

    [Fact(DisplayName = "Given valid data should create a sale")]
    public async Task Handle_WhenValidData_ShouldCreateSale()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var productPrice = 100m;

        var command = new CreateSaleCommand
        {
            CustomerId = customerId,
            Items =
            [
                new SaleItemDto
            {
                ProductId = productId,
                Quantity = 2
            }
            ]
        };

        _customerRepoMock.Setup(r => r.Get(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
            .Returns(new CustomerEntity { Id = customerId });

        _productRepoMock.Setup(r => r.Query())
            .Returns(new List<ProductEntity>
            {
                new() { Id = productId, Price = productPrice }
            }.AsQueryable());

        _saleRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<SaleEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SaleEntity sale, CancellationToken _) => sale);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CustomerId.Should().Be(customerId);
        result.Total.Should().Be(2 * productPrice);
    }

    [Fact(DisplayName = "Given invalid data should throw BadRequestException")]
    public async Task Handle_WhenInvalidData_ShouldThrowBadRequestException()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            Items = []
        };

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact(DisplayName = "Given non-existent customer should throw NotFoundException")]
    public async Task Handle_WhenCustomerNotFound_ShouldThrowNotFoundException()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            Items =
            [
                new SaleItemDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 1
                }
            ]
        };

        _customerRepoMock.Setup(r => r.Get(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
            .Returns((CustomerEntity?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact(DisplayName = "Given non-existent product should throw NotFoundException")]
    public async Task Handle_WhenProductNotFound_ShouldThrowNotFoundException()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var command = new CreateSaleCommand
        {
            CustomerId = customerId,
            Items =
            [
                new SaleItemDto
                {
                    ProductId = productId,
                    Quantity = 1
                }
            ]
        };

        _customerRepoMock.Setup(r => r.Get(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
            .Returns(new CustomerEntity { Id = customerId });

        _productRepoMock.Setup(r => r.Get(It.IsAny<Expression<Func<ProductEntity, bool>>>()))
            .Returns((ProductEntity?)null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}