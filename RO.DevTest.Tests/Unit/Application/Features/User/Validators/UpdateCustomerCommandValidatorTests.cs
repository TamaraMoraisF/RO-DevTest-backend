using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Customer.Commands.UpdateCustomerCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.User.Validators;

public class UpdateCustomerCommandValidatorTests
{
    private readonly UpdateCustomerCommandValidator _validator;

    public UpdateCustomerCommandValidatorTests()
    {
        _validator = new UpdateCustomerCommandValidator();
    }

    [Fact(DisplayName = "Given valid data should not have validation errors")]
    public void Should_NotHaveValidationErrors_WhenDataIsValid()
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

    [Fact(DisplayName = "Given invalid name and email should have validation errors")]
    public void Should_HaveValidationErrors_WhenNameAndEmailAreInvalid()
    {
        var command = new UpdateCustomerWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "",
            Email = "invalid-email"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact(DisplayName = "Given invalid ID should have validation error")]
    public void Should_HaveValidationError_WhenIdIsZero()
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

    [Fact(DisplayName = "Given invalid Name should have validation error")]
    public void Should_HaveValidationError_WhenNameIsrequired()
    {
        var command = new UpdateCustomerWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "",
            Email = "ana@email.com"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}