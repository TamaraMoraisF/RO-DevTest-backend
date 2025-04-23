using FluentValidation;

namespace RO.DevTest.Application.Features.User.Commands.CreateUserCommand;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(cpau => cpau.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("The email field must be filled.");

        RuleFor(cpau => cpau.Email)
            .EmailAddress()
            .WithMessage("The email field must be a valid email address.");

        RuleFor(cpau => cpau.Password)
            .MinimumLength(6)
            .WithMessage("The password must be at least 6 characters long.");

        RuleFor(cpau => cpau.PasswordConfirmation)
            .Matches(cpau => cpau.Password)
            .WithMessage("The password confirmation must match the password field.");
    }
}