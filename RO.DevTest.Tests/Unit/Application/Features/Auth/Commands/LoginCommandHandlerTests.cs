using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using RO.DevTest.Application.Features.Auth.Commands.LoginCommand;
using AppUser = RO.DevTest.Domain.Entities.User;

namespace RO.DevTest.Tests.Unit.Application.Features.Auth.Commands;

public class LoginCommandHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly LoginCommandHandler _sut;

    public LoginCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<AppUser>>();

        _userManagerMock = new Mock<UserManager<AppUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<Domain.Entities.User>>();

        _signInManagerMock = new Mock<SignInManager<AppUser>>(
            _userManagerMock.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            null, null, null, null);

        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("testing-jwt-key-12345678901234567890123456789012");

        _sut = new LoginCommandHandler(
            _signInManagerMock.Object,
            _userManagerMock.Object,
            _configurationMock.Object);
    }

    [Fact(DisplayName = "Given invalid credentials should throw UnauthorizedAccessException")]
    public async Task Handle_WhenUserNotFound_ShouldThrowUnauthorizedAccess()
    {
        // Arrange
        var command = new LoginCommand { Username = "invalid", Password = "123" };
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null);

        // Act
        Func<Task> act = async () => await _sut.Handle(command, new CancellationToken());

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact(DisplayName = "Given valid credentials should return a valid JWT token")]
    public async Task Handle_WhenCredentialsAreValid_ShouldReturnAccessToken()
    {
        // Arrange
        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Ana"
        };
        var command = new LoginCommand { Username = user.UserName!, Password = "123456" };

        _userManagerMock.Setup(m => m.FindByNameAsync(user.UserName)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, command.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(["Admin"]);

        // Act
        var result = await _sut.Handle(command, new CancellationToken());

        // Assert
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.Roles.Should().Contain("Admin");
        result.IssuedAt.Should().BeBefore(result.ExpirationDate);
    }
}