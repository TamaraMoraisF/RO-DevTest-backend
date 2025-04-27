using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Customer.Commands.CreateCustomerCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.Customer.Validators;

public class CreateCustomerCommandValidatorTests
{
    private readonly CreateCustomerCommandValidator _validator;

    public CreateCustomerCommandValidatorTests()
    {
        _validator = new CreateCustomerCommandValidator();
    }

    [Fact(DisplayName = "Should not have validation error when all fields are valid")]
    public void Should_NotHaveValidationErrors_WhenValid()
    {
        // Arrange
        var command = new CreateCustomerCommand { Name = "Ana", Email = "ana@email.com" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should have error when name is empty")]
    public void Should_HaveError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateCustomerCommand { Name = "", Email = "ana@email.com" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact(DisplayName = "Should have error when email is invalid")]
    public void Should_HaveError_WhenEmailIsInvalid()
    {
        // Arrange
        var command = new CreateCustomerCommand { Name = "Ana", Email = "invalid-email" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}
