using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Customer.Commands.CreateCustomerCommand;
using RO.DevTest.Domain.Exception;
using CustomerEntity = RO.DevTest.Domain.Entities.Customer;

namespace RO.DevTest.Tests.Unit.Application.Features.Customer.Commands;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _customerRepoMock = new();
        _handler = new CreateCustomerCommandHandler(_customerRepoMock.Object);
    }

    [Fact(DisplayName = "Given valid data should create a customer")]
    public async Task Handle_WhenValidData_ShouldCreateCustomer()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            Name = "Ana",
            Email = "ana@email.com"
        };

        _customerRepoMock.Setup(repo => repo.CreateAsync(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CustomerEntity c, CancellationToken _) => c);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Email.Should().Be(command.Email);
    }

    [Fact(DisplayName = "Given invalid data should throw BadRequestException")]
    public async Task Handle_WhenInvalidData_ShouldThrowBadRequestException()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            Name = "",
            Email = "invalido"
        };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }
}
