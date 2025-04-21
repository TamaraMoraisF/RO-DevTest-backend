using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Application.Features.User.Commands.CreateUserCommand;
using RO.DevTest.Domain.Enums;
using RO.DevTest.Domain.Exception;
using AppUser = RO.DevTest.Domain.Entities.User;

namespace RO.DevTest.Tests.Unit.Application.Features.User.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IIdentityAbstractor> _identityAbstractorMock = new();
    private readonly CreateUserCommandHandler _sut;

    public CreateUserCommandHandlerTests()
    {
        _sut = new(_identityAbstractorMock.Object);
    }

    [Fact(DisplayName = "Given invalid email should throw a BadRequestException")]
    public void Handle_WhenEmailIsNullOrEmpty_ShouldRaiseABadRequestExcpetion()
    {
        // Arrange
        string email = string.Empty, password = Guid.NewGuid().ToString();
        CreateUserCommand command = new()
        {
            Email = email,
            UserName = "user_test",
            Password = password,
            PasswordConfirmation = password,
            Name = "Test User"
        };

        // Act
        Func<Task> action = async () => await _sut.Handle(command, new CancellationToken());

        // Assert
        action.Should().ThrowAsync<BadRequestException>();
    }

    [Fact(DisplayName = "Given passwords not matching should throw a BadRequestException")]
    public void Handle_WhenPasswordDoesntMatchPasswordConfirmation_ShouldRaiseABadRequestException()
    {
        // Arrange
        string email = "mytestemail@someprovider.com"
            , password = Guid.NewGuid().ToString()
            , passwordConfirmation = Guid.NewGuid().ToString();
        CreateUserCommand command = new()
        {
            Email = email,
            UserName = "user_test",
            Password = password,
            PasswordConfirmation = passwordConfirmation,
            Name = "Test User"
        };

        // Act
        Func<Task> action = async () => await _sut.Handle(command, new CancellationToken());

        // Assert
        action.Should().ThrowAsync<BadRequestException>();
    }

    [Fact(DisplayName = "Given successful user creation should return CreateUserResult")]
    public async Task Handle_WhenDataIsValid_ShouldReturnCreateUserResult()
    {
        // Arrange
        var password = "StrongPassword123!";
        var user = new AppUser { Email = "valid@test.com", UserName = "user_test", Name = "User" };

        CreateUserCommand command = new()
        {
            Email = user.Email,
            UserName = user.UserName,
            Password = password,
            PasswordConfirmation = password,
            Name = user.Name,
            Role = UserRoles.Admin
        };

        _identityAbstractorMock.Setup(x => x.CreateUserAsync(It.IsAny<AppUser>(), password))
            .ReturnsAsync(IdentityResult.Success);

        _identityAbstractorMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Admin))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(command.Email);
        result.UserName.Should().Be(command.UserName);
        result.Name.Should().Be(command.Name);
    }

    [Fact(DisplayName = "Given failed user creation should throw BadRequestException")]
    public async Task Handle_WhenUserCreationFails_ShouldRaiseBadRequestException()
    {
        // Arrange
        var password = "StrongPassword123!";
        CreateUserCommand command = new()
        {
            Email = "test@domain.com",
            UserName = "testuser",
            Password = password,
            PasswordConfirmation = password,
            Name = "Test",
            Role = UserRoles.Admin
        };

        _identityAbstractorMock.Setup(x => x.CreateUserAsync(It.IsAny<AppUser>(), password))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact(DisplayName = "Given role assignment fails should throw BadRequestException")]
    public async Task Handle_WhenRoleAssignmentFails_ShouldRaiseBadRequestException()
    {
        // Arrange
        var password = "StrongPassword123!";
        CreateUserCommand command = new()
        {
            Email = "test@domain.com",
            UserName = "testuser",
            Password = password,
            PasswordConfirmation = password,
            Name = "Test",
            Role = UserRoles.Admin
        };

        _identityAbstractorMock.Setup(x => x.CreateUserAsync(It.IsAny<AppUser>(), password))
            .ReturnsAsync(IdentityResult.Success);

        _identityAbstractorMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), UserRoles.Admin))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }
}
