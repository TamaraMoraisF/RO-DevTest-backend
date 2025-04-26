using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;
using CustomerEntity = RO.DevTest.Domain.Entities.Customer;
using ProductEntity = RO.DevTest.Domain.Entities.Product;
using SaleEntity = RO.DevTest.Domain.Entities.Sale;
using SaleItemEntity = RO.DevTest.Domain.Entities.SaleItem;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Commands;

public class GetSalesAnalyticsCommandHandlerTests
{
    private readonly Mock<ISaleRepository> _saleRepoMock;
    private readonly GetSalesAnalyticsCommandHandler _handler;

    public GetSalesAnalyticsCommandHandlerTests()
    {
        _saleRepoMock = new();

        var fakeSales = new List<SaleEntity>
        {
            new()
            {
                SaleDate = new DateTime(2025, 4, 24),
                Customer = new CustomerEntity { Name = "Ana" },
                Items =
                {
                    new SaleItemEntity
                    {
                        Product = new ProductEntity { Id = Guid.NewGuid(), Name = "Produto A" },
                        Quantity = 2,
                        UnitPrice = 50
                    },
                    new SaleItemEntity
                    {
                        Product = new ProductEntity { Id = Guid.NewGuid(), Name = "Produto B" },
                        Quantity = 1,
                        UnitPrice = 100
                    }
                }
            },
            new()
            {
                SaleDate = new DateTime(2025, 4, 24),
                Customer = new CustomerEntity { Name = "Bruno" },
                Items =
                {
                    new SaleItemEntity
                    {
                        Product = new ProductEntity { Id = Guid.NewGuid(), Name = "Produto A" },
                        Quantity = 1,
                        UnitPrice = 50
                    }
                }
            }
        }.AsQueryable();

        _saleRepoMock.Setup(r => r.Query()).Returns(fakeSales);
        _handler = new GetSalesAnalyticsCommandHandler(_saleRepoMock.Object);
    }

    [Fact]
    public async Task ShouldReturnCorrectTotalSalesAndRevenue()
    {
        var query = new GetSalesAnalyticsCommand(
            new DateTime(2025, 4, 24),
            new DateTime(2025, 4, 24)
        );

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalSales.Should().Be(2);
        result.TotalRevenue.Should().Be(250);
        result.ProductRevenueBreakdown.Should().HaveCount(2);
    }

    [Fact]
    public async Task ShouldReturnEmptyWhenNoSalesInPeriod()
    {
        _saleRepoMock.Setup(r => r.Query()).Returns(Enumerable.Empty<SaleEntity>().AsQueryable());

        var query = new GetSalesAnalyticsCommand(
            new DateTime(2025, 1, 1),
            new DateTime(2025, 1, 1)
        );

        var result = await _handler.Handle(query, CancellationToken.None);

        result.TotalSales.Should().Be(0);
        result.TotalRevenue.Should().Be(0);
        result.ProductRevenueBreakdown.Should().BeEmpty();
    }
}