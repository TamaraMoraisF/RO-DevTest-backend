using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Commands;

public class GetSalesAnalyticsCommandValidatorTests
{
    private readonly GetSalesAnalyticsCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_StartDate_Is_After_EndDate()
    {
        var command = new GetSalesAnalyticsCommand(DateTime.UtcNow.AddDays(1), DateTime.UtcNow);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void Should_Have_Error_When_StartDate_Is_Default()
    {
        var command = new GetSalesAnalyticsCommand(default, DateTime.UtcNow);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Start);
    }

    [Fact]
    public void Should_Have_Error_When_EndDate_Is_Default()
    {
        var command = new GetSalesAnalyticsCommand(DateTime.UtcNow, default);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.End);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Dates_Are_Valid()
    {
        var command = new GetSalesAnalyticsCommand(DateTime.UtcNow.AddDays(-5), DateTime.UtcNow);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}