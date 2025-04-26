using MediatR;

namespace RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

public class LoginCommand : IRequest<LoginResult> {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
