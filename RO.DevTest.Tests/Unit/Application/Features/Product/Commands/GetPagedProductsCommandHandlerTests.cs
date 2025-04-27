using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Commands.GetPagedProductsCommand;
using ProductEntity = RO.DevTest.Domain.Entities.Product;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Commands;

public class GetPagedProductsCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly GetPagedProductsCommandHandler _handler;

    public GetPagedProductsCommandHandlerTests()
    {
        _productRepoMock = new();

        var fakeData = new List<ProductEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Notebook", Price = 2500 },
            new() { Id = Guid.NewGuid(), Name = "Mouse", Price = 50 },
            new() { Id = Guid.NewGuid(), Name = "Monitor", Price = 800 },
            new() { Id = Guid.NewGuid(), Name = "Teclado", Price = 120 },
            new() { Id = Guid.NewGuid(), Name = "Notebook Gamer", Price = 6000 },
        }.AsQueryable();

        _productRepoMock.Setup(repo => repo.Query())
            .Returns(fakeData);

        _handler = new GetPagedProductsCommandHandler(_productRepoMock.Object);
    }

    [Fact(DisplayName = "Given search 'note', when querying, should return filtered products")]
    public async Task Handle_GivenSearchTerm_ShouldFilterProducts()
    {
        // Arrange
        var query = new GetPagedProductsCommand
        {
            Search = "note",
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalItems.Should().Be(2);
        result.Items.Should().OnlyContain(p => p.Name.ToLower().Contains("note"));
    }

    [Fact(DisplayName = "Given Page = 1 and PageSize = 2, should return first 2 items")]
    public async Task Handle_GivenPaginationPage1_ShouldReturnFirstItems()
    {
        // Arrange
        var query = new GetPagedProductsCommand
        {
            Page = 1,
            PageSize = 2
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalItems.Should().Be(5);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(2);
        result.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Given Page = 2 and PageSize = 2, should return remaining products")]
    public async Task Handle_GivenPaginationPage2_ShouldReturnNextItems()
    {
        // Arrange
        var query = new GetPagedProductsCommand
        {
            Page = 2,
            PageSize = 2
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalItems.Should().Be(5);
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Given SortBy 'name' ascending, should return sorted result")]
    public async Task Handle_GivenSortByNameAsc_ShouldSortCorrectly()
    {
        // Arrange
        var query = new GetPagedProductsCommand
        {
            SortBy = "name",
            Descending = false,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeInAscendingOrder(p => p.Name);
    }

    [Fact(DisplayName = "Given SortBy 'price' descending, should return sorted result")]
    public async Task Handle_GivenSortByPriceDesc_ShouldSortCorrectly()
    {
        // Arrange
        var query = new GetPagedProductsCommand
        {
            SortBy = "price",
            Descending = true,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeInDescendingOrder(p => p.Price);
    }
}
