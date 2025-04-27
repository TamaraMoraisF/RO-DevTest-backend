using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Validators;

public class DeleteProductCommandValidatorTests
{
    private readonly DeleteProductCommandValidator _validator;

    public DeleteProductCommandValidatorTests()
    {
        _validator = new DeleteProductCommandValidator();
    }

    [Fact(DisplayName = "Should not have validation error when ID is valid")]
    public void Should_NotHaveError_WhenIdIsValid()
    {
        // Arrange
        var command = new DeleteProductCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should have error when ID is empty")]
    public void Should_HaveError_WhenIdIsEmpty()
    {
        // Arrange
        var command = new DeleteProductCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}
