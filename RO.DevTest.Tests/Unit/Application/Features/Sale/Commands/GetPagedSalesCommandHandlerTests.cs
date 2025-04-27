using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;
using CustomerEntity = RO.DevTest.Domain.Entities.Customer;
using ProductEntity = RO.DevTest.Domain.Entities.Product;
using SaleEntity = RO.DevTest.Domain.Entities.Sale;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Commands;

public class GetPagedSalesCommandHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepoMock;
    private readonly GetPagedSalesCommandHandler _handler;

    public GetPagedSalesCommandHandlerTests()
    {
        _saleRepoMock = new();

        var fakeSales = new List<SaleEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                SaleDate = new DateTime(2025, 4, 24),
                Customer = new CustomerEntity { Name = "João" },
                Items = [ new() { Product = new ProductEntity { Name = "Faqueiro" }, Quantity = 2, UnitPrice = 70 } ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                SaleDate = new DateTime(2025, 4, 23),
                Customer = new CustomerEntity { Name = "Ana" },
                Items = [ new() { Product = new ProductEntity { Name = "Liquidificador" }, Quantity = 1, UnitPrice = 100 } ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                SaleDate = new DateTime(2025, 4, 22),
                Customer = new CustomerEntity { Name = "Bruno" },
                Items = [ new() { Product = new ProductEntity { Name = "Panela" }, Quantity = 1, UnitPrice = 200 } ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                SaleDate = new DateTime(2025, 4, 21),
                Customer = new CustomerEntity { Name = "Carlos" },
                Items = [ new() { Product = new ProductEntity { Name = "Cadeira" }, Quantity = 2, UnitPrice = 150 } ]
            }
        }.AsQueryable();

        _saleRepoMock.Setup(r => r.Query()).Returns(fakeSales);

        _handler = new GetPagedSalesCommandHandler(_saleRepoMock.Object);
    }

    [Fact(DisplayName = "Given sales, when paginating, should return correct page")]
    public async Task Handle_WhenPaginating_ShouldReturnCorrectPage()
    {
        // Arrange
        var query = new GetPagedSalesCommand
        {
            Page = 1,
            PageSize = 2
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalItems.Should().Be(4);
        result.TotalPages.Should().Be(2);
    }

    [Fact(DisplayName = "Given sales, when paginating to page 2, should return remaining items")]
    public async Task Handle_WhenPaginatingToPage2_ShouldReturnRemainingItems()
    {
        // Arrange
        var query = new GetPagedSalesCommand
        {
            Page = 2,
            PageSize = 2
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Given sales, when sorting by customer ascending, should return ordered list")]
    public async Task Handle_WhenSortingByCustomerAsc_ShouldReturnOrdered()
    {
        // Arrange
        var query = new GetPagedSalesCommand
        {
            SortBy = "customer",
            Descending = false,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeInAscendingOrder(s => s.CustomerName);
    }

    [Fact(DisplayName = "Given sales, when sorting by sale date descending, should return ordered list")]
    public async Task Handle_WhenSortingBySaleDateDesc_ShouldReturnOrdered()
    {
        // Arrange
        var query = new GetPagedSalesCommand
        {
            SortBy = "saledate",
            Descending = true,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeInDescendingOrder(s => s.SaleDate);
    }

    [Fact(DisplayName = "Given sales, when sorting by total descending, should return ordered list")]
    public async Task Handle_WhenSortingByTotalDesc_ShouldReturnOrdered()
    {
        // Arrange
        var query = new GetPagedSalesCommand
        {
            SortBy = "total",
            Descending = true,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeInDescendingOrder(s => s.Total);
    }
}
