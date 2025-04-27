using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.User.Validators;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _validator = new CreateUserCommandValidator();
    }

    [Fact(DisplayName = "Should have validation error when email is empty")]
    public void Should_HaveError_WhenEmailIsEmpty()
    {
        // Arrange
        var command = new CreateUserCommand { Email = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact(DisplayName = "Should have validation error when email is invalid")]
    public void Should_HaveError_WhenEmailIsInvalid()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "invalid-email" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact(DisplayName = "Should have validation error when password is too short")]
    public void Should_HaveError_WhenPasswordIsShort()
    {
        // Arrange
        var command = new CreateUserCommand { Password = "123" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact(DisplayName = "Should pass when PasswordConfirmation matches Password (with Matches)")]
    public void Should_NotHaveValidationError_WhenPasswordConfirmationMatches()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "user@email.com",
            Password = "MyPassword123",
            PasswordConfirmation = "MyPassword123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PasswordConfirmation);
    }

    [Fact(DisplayName = "Should fail when PasswordConfirmation does not match Password (with Matches)")]
    public void Should_HaveValidationError_WhenPasswordConfirmationIsDifferent()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "user@email.com",
            Password = "MyPassword123",
            PasswordConfirmation = "Different123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PasswordConfirmation);
    }

    [Fact(DisplayName = "Should pass when all fields are valid")]
    public void Should_NotHaveError_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "valid@email.com",
            Password = "Password123",
            PasswordConfirmation = "Password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
