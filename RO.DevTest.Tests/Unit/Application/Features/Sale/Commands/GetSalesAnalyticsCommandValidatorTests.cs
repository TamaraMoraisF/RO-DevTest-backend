using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;

namespace RO.DevTest.Tests.Unit.Application.Features.Sale.Commands;

public class GetSalesAnalyticsCommandValidatorTests
{
    private readonly GetSalesAnalyticsCommandValidator _validator = new();

    [Fact(DisplayName = "Should have error when StartDate is after EndDate")]
    public void Should_Have_Error_When_StartDate_Is_After_EndDate()
    {
        // Arrange
        var command = new GetSalesAnalyticsCommand(DateTime.UtcNow.AddDays(1), DateTime.UtcNow);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact(DisplayName = "Should have error when StartDate is default")]
    public void Should_Have_Error_When_StartDate_Is_Default()
    {
        // Arrange
        var command = new GetSalesAnalyticsCommand(default, DateTime.UtcNow);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Start);
    }

    [Fact(DisplayName = "Should have error when EndDate is default")]
    public void Should_Have_Error_When_EndDate_Is_Default()
    {
        // Arrange
        var command = new GetSalesAnalyticsCommand(DateTime.UtcNow, default);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.End);
    }

    [Fact(DisplayName = "Should not have error when dates are valid")]
    public void Should_Not_Have_Error_When_Dates_Are_Valid()
    {
        // Arrange
        var command = new GetSalesAnalyticsCommand(DateTime.UtcNow.AddDays(-5), DateTime.UtcNow);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
