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
        var command = new DeleteCustomerCommand { Id = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact(DisplayName = "Should not have error when ID is valid")]
    public void Should_NotHaveError_WhenIdIsValid()
    {
        var command = new DeleteCustomerCommand { Id = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}