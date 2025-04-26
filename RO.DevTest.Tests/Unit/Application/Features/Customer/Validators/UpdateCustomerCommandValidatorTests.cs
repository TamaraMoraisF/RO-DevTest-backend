using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Customer.Commands.UpdateCustomerCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.Customer.Validators;

public class UpdateCustomerCommandValidatorTests
{
    private readonly UpdateCustomerCommandValidator _validator;

    public UpdateCustomerCommandValidatorTests()
    {
        _validator = new UpdateCustomerCommandValidator();
    }

    [Fact(DisplayName = "Should validate successfully with all fields valid")]
    public void Should_NotHaveErrors_WhenValid()
    {
        var command = new UpdateCustomerWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "Ana",
            Email = "ana@email.com"
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should have error when ID is empty")]
    public void Should_HaveError_WhenIdIsEmpty()
    {
        var command = new UpdateCustomerWithIdCommand
        {
            Id = Guid.Empty,
            Name = "Ana",
            Email = "ana@email.com"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact(DisplayName = "Should have error when email is invalid")]
    public void Should_HaveError_WhenEmailIsInvalid()
    {
        var command = new UpdateCustomerWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "Ana",
            Email = "invalid"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}