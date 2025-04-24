using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Product.Queries.GetPagedProducts;
using ProductEntity = RO.DevTest.Domain.Entities.Product;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Queries;

public class GetPagedProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly GetPagedProductsQueryHandler _handler;

    public GetPagedProductsQueryHandlerTests()
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

        _handler = new GetPagedProductsQueryHandler(_productRepoMock.Object);
    }

    [Fact(DisplayName = "Given search 'note', when querying, should return filtered products")]
    public async Task Handle_GivenSearchTerm_ShouldFilterProducts()
    {
        var query = new GetPagedProductsQuery
        {
            Search = "note",
            Page = 1,
            PageSize = 10
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalItems.Should().Be(2);
        result.Items.Should().OnlyContain(p => p.Name.ToLower().Contains("note"));
    }

    [Fact(DisplayName = "Given Page = 1 and PageSize = 2, should return first 2 items")]
    public async Task Handle_GivenPaginationPage1_ShouldReturnFirstItems()
    {
        var query = new GetPagedProductsQuery
        {
            Page = 1,
            PageSize = 2
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalItems.Should().Be(5);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(2);
        result.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Given Page = 2 and PageSize = 2, should return remaining products")]
    public async Task Handle_GivenPaginationPage2_ShouldReturnNextItems()
    {
        var query = new GetPagedProductsQuery
        {
            Page = 2,
            PageSize = 2
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalItems.Should().Be(5);
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Given SortBy 'name' ascending, should return sorted result")]
    public async Task Handle_GivenSortByNameAsc_ShouldSortCorrectly()
    {
        var query = new GetPagedProductsQuery
        {
            SortBy = "name",
            Descending = false,
            Page = 1,
            PageSize = 10
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Items.Should().BeInAscendingOrder(p => p.Name);
    }

    [Fact(DisplayName = "Given SortBy 'price' descending, should return sorted result")]
    public async Task Handle_GivenSortByPriceDesc_ShouldSortCorrectly()
    {
        var query = new GetPagedProductsQuery
        {
            SortBy = "price",
            Descending = true,
            Page = 1,
            PageSize = 10
        };

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Items.Should().BeInDescendingOrder(p => p.Price);
    }
}