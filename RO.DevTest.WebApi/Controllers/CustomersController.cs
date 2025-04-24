using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Features.Customer.Commands.CreateCustomerCommand;
using RO.DevTest.Application.Features.Customer.Commands.DeleteCustomerCommand;
using RO.DevTest.Application.Features.Customer.Commands.UpdateCustomerCommand;
using RO.DevTest.Application.Features.Customer.Queries.GetPagedCustomers;
using RO.DevTest.Application.Models;

namespace RO.DevTest.WebApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/customers")]
[ApiController]
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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteCustomerCommand { Id = id });
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<CustomerResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetPagedCustomersQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}