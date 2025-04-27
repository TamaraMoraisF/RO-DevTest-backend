using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Product.Commands.GetPagedProductsCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Commands;

public class GetPagedProductsCommandValidatorTests
{
    private readonly GetPagedProductsCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Page_Is_Zero()
    {
        // Arrange
        var command = new GetPagedProductsCommand { Page = 0, PageSize = 10 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Zero()
    {
        // Arrange
        var command = new GetPagedProductsCommand { Page = 1, PageSize = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Greater_Than_100()
    {
        // Arrange
        var command = new GetPagedProductsCommand { Page = 1, PageSize = 101 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Command()
    {
        // Arrange
        var command = new GetPagedProductsCommand { Page = 1, PageSize = 20 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
