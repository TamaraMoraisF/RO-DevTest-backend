using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        try
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
}
