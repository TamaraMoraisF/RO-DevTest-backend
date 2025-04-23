using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Customer.Commands.DeleteCustomerCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.User.Validators;

public class DeleteCustomerCommandValidatorTests
{
    private readonly DeleteCustomerCommandValidator _validator;

    public DeleteCustomerCommandValidatorTests()
    {
        _validator = new DeleteCustomerCommandValidator();
    }

    [Fact(DisplayName = "Given valid ID should not have validation error")]
    public void Should_NotHaveValidationErrors_WhenIdIsValid()
    {
        var command = new DeleteCustomerCommand { Id = Guid.NewGuid() };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Given zero ID should have validation error")]
    public void Should_HaveValidationError_WhenIdIsZero()
    {
        var command = new DeleteCustomerCommand { Id = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}
