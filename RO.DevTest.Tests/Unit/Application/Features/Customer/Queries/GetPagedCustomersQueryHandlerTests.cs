using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Customer.Queries.GetPagedCustomers;
using CustomerEntity = RO.DevTest.Domain.Entities.Customer;

namespace RO.DevTest.Tests.Unit.Application.Features.Customer.Queries;

public class GetPagedCustomersQueryHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly GetPagedCustomersQueryHandler _handler;

    public GetPagedCustomersQueryHandlerTests()
    {
        _customerRepoMock = new();

        var fakeData = new List<CustomerEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Ana Clara", Email = "ana@email.com" },
            new() { Id = Guid.NewGuid(), Name = "Bruno Silva", Email = "bruno@email.com" },
            new() { Id = Guid.NewGuid(), Name = "Mariana Souza", Email = "mari@email.com" },
        }.AsQueryable();

        _customerRepoMock.Setup(repo => repo.Query())
            .Returns(fakeData);

        _handler = new GetPagedCustomersQueryHandler(_customerRepoMock.Object);
    }

    [Fact(DisplayName = "Given search term 'ana', when querying, should return filtered and paged result")]
    public async Task Handle_GivenSearchTerm_ShouldFilterAndPaginate()
    {
        // Arrange
        var query = new GetPagedCustomersQuery
        {
            Search = "ana",
            Page = 1,
            PageSize = 2
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalItems.Should().Be(2);
        result.Items.Should().HaveCount(2);
        result.Items.Should().OnlyContain(c =>
            c.Name.Contains("ana", StringComparison.OrdinalIgnoreCase) ||
            c.Email.Contains("ana", StringComparison.OrdinalIgnoreCase));
    }

    [Fact(DisplayName = "Given no search term, when querying, should return all paged results")]
    public async Task Handle_WhenNoSearchTerm_ShouldReturnAllPaged()
    {
        // Arrange
        var query = new GetPagedCustomersQuery
        {
            Page = 1,
            PageSize = 2
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalItems.Should().Be(3);
        result.Items.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Given sort by 'name' ascending, should return ordered results")]
    public async Task Handle_SortByNameAscending_ShouldOrderResults()
    {
        // Arrange
        var query = new GetPagedCustomersQuery
        {
            SortBy = "name",
            Descending = false,
            Page = 1,
            PageSize = 3
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeInAscendingOrder(c => c.Name);
    }

    [Fact(DisplayName = "Given sort by 'email' descending, should return ordered results")]
    public async Task Handle_SortByEmailDescending_ShouldOrderResults()
    {
        // Arrange
        var query = new GetPagedCustomersQuery
        {
            SortBy = "email",
            Descending = true,
            Page = 1,
            PageSize = 3
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeInDescendingOrder(c => c.Email);
    }

    [Fact(DisplayName = "Given page and pageSize, should return correct subset of results")]
    public async Task Handle_WhenPaged_ShouldReturnCorrectPage()
    {
        // Arrange
        var query = new GetPagedCustomersQuery
        {
            Page = 2,
            PageSize = 2
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.TotalItems.Should().Be(3);
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.Items.Should().HaveCount(1);
    }
}