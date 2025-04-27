using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Customer.Commands.DeleteCustomerCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.Customer.Validators;

public class DeleteCustomerCommandValidatorTests
{
    private readonly DeleteCustomerCommandValidator _validator;

    public DeleteCustomerCommandValidatorTests()
    {
        _validator = new DeleteCustomerCommandValidator();
    }

    [Fact(DisplayName = "Should have error when ID is empty")]
    public void Should_HaveError_WhenIdIsEmpty()
    {
        // Arrange
        var command = new DeleteCustomerCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact(DisplayName = "Should not have error when ID is valid")]
    public void Should_NotHaveError_WhenIdIsValid()
    {
        // Arrange
        var command = new DeleteCustomerCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
