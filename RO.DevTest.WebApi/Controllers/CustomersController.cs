using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RO.DevTest.Application.Features.Customer.Commands.CreateCustomerCommand;

namespace RO.DevTest.WebApi.Controllers;

[Route("api/customers")]
[ApiController]
[OpenApiTags("Customers")]
public class CustomersController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CreateCustomerResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateCustomerCommand request)
    {
        var result = await _mediator.Send(request);
        return CreatedAtAction(nameof(Create), result);
    }
}
