using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RO.DevTest.Application.Features.Customer.Commands.CreateCustomerCommand;
using RO.DevTest.Application.Features.Customer.Commands.UpdateCustomerCommand;

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

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateCustomerResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerCommand request)
    {
        var command = new UpdateCustomerWithIdCommand
        {
            Id = id,
            Name = request.Name,
            Email = request.Email
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}