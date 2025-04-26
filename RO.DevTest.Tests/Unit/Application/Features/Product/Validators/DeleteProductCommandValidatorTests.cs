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
        var command = new DeleteProductCommand { Id = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should have error when ID is empty")]
    public void Should_HaveError_WhenIdIsEmpty()
    {
        var command = new DeleteProductCommand { Id = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}
