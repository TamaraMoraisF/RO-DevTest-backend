using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

namespace RO.DevTest.WebApi.Controllers;

[Route("api/auth")]
[OpenApiTags("Auth")]
[ApiController]
public class AuthController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
