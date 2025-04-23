using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Validators;

public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator;

    public CreateSaleCommandValidatorTests()
    {
        _validator = new CreateSaleCommandValidator();
    }

    [Fact(DisplayName = "Should pass when command is valid")]
    public void Should_NotHaveErrors_WhenCommandIsValid()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            Items =
            [
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    UnitPrice = 100
                }
            ]
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should fail when CustomerId is empty")]
    public void Should_HaveError_WhenCustomerIdIsEmpty()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            Items =
            [
                new()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    UnitPrice = 100
                }
            ]
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }

    [Fact(DisplayName = "Should fail when items list is empty")]
    public void Should_HaveError_WhenItemsAreEmpty()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            Items = []
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Items);
    }

    [Fact(DisplayName = "Should fail when item fields are invalid")]
    public void Should_HaveErrors_WhenItemFieldsAreInvalid()
    {
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            Items =
            [
                new()
                {
                    ProductId = Guid.Empty,
                    Quantity = 0,
                    UnitPrice = -1
                }
            ]
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Items[0].ProductId");
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
        result.ShouldHaveValidationErrorFor("Items[0].UnitPrice");
    }
}