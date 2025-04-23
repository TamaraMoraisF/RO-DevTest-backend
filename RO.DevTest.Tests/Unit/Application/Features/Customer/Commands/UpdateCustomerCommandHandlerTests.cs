using FluentAssertions;
using Moq;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Customer.Commands.UpdateCustomerCommand;
using RO.DevTest.Domain.Exception;
using System.Linq.Expressions;
using CustomerEntity = RO.DevTest.Domain.Entities.Customer;

namespace RO.DevTest.Tests.Unit.Application.Features.Customer.Commands;

public class UpdateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockRepo;
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerCommandHandlerTests()
    {
        _mockRepo = new Mock<ICustomerRepository>();
        _handler = new UpdateCustomerCommandHandler(_mockRepo.Object);
    }

    [Fact(DisplayName = "Given valid data should update a customer")]
    public async Task Should_Update_Customer_When_Data_Is_Valid()
    {
        // Arrange
        var command = new UpdateCustomerWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Ana",
            Email = "ana.updated@email.com"
        };

        var existingCustomer = new CustomerEntity
        {
            Id = command.Id,
            Name = "Old Ana",
            Email = "old@email.com"
        };

        _mockRepo.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                 .Returns(existingCustomer);

        _mockRepo.Setup(repo => repo.Update(It.IsAny<CustomerEntity>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Email.Should().Be(command.Email);
    }

    [Fact(DisplayName = "Given non-existent customer should throw NotFoundException")]
    public async Task Should_Throw_NotFoundException_When_Customer_Not_Found()
    {
        // Arrange
        var command = new UpdateCustomerWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "Ana",
            Email = "notfound@email.com"
        };

        _mockRepo.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomerEntity, bool>>>()))
                 .Returns((CustomerEntity)null!);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact(DisplayName = "Given invalid data should throw BadRequestException")]
    public async Task Should_Throw_BadRequestException_When_Validation_Fails()
    {
        // Arrange
        var command = new UpdateCustomerWithIdCommand
        {
            Id = Guid.Empty,
            Name = "",
            Email = "invalid-email"
        };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }
}