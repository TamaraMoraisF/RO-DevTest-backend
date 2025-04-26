using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Commands;

public class GetPagedSalesCommandValidatorTests
{
    private readonly GetPagedSalesCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Page_Is_Zero()
    {
        var command = new GetPagedSalesCommand { Page = 0, PageSize = 10 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Zero()
    {
        var command = new GetPagedSalesCommand { Page = 1, PageSize = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Greater_Than_100()
    {
        var command = new GetPagedSalesCommand { Page = 1, PageSize = 101 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Values()
    {
        var command = new GetPagedSalesCommand { Page = 1, PageSize = 20 };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}