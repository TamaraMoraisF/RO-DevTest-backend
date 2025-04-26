using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Customer.Commands.DeleteCustomerCommand;
using RO.DevTest.Domain.Exception;
using System.Linq.Expressions;
using CustomerEntity = RO.DevTest.Domain.Entities.Customer;

namespace RO.DevTest.Tests.Unit.Application.Features.Customer.Commands;

public class DeleteCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly DeleteCustomerCommandHandler _handler;

    public DeleteCustomerCommandHandlerTests()
    {
        _customerRepoMock = new();
        _handler = new DeleteCustomerCommandHandler(_customerRepoMock.Object);
    }

    [Fact(DisplayName = "Given valid ID should delete the customer")]
    public async Task Handle_WhenValidId_ShouldDeleteCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var existingCustomer = new CustomerEntity
        {
            Id = customerId,
            Name = "Ana",
            Email = "ana@email.com"
        };

        _customerRepoMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                 .Returns(existingCustomer);

        _customerRepoMock.Setup(repo => repo.Delete(existingCustomer, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new DeleteCustomerCommand { Id = customerId };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _customerRepoMock.Verify(repo => repo.Delete(existingCustomer, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Given non-existent ID should throw NotFoundException")]
    public async Task Handle_WhenCustomerNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new DeleteCustomerCommand { Id = Guid.NewGuid() };


        _customerRepoMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                 .Returns((CustomerEntity)null!);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}