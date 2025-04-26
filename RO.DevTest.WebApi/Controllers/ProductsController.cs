using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;
using RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;
using RO.DevTest.Application.Features.Product.Commands.GetPagedProductsCommand;
using RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;
using RO.DevTest.Application.Models;

namespace RO.DevTest.WebApi.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CreateProductResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateProductCommand request)
    {
        var result = await _mediator.Send(request);
        return CreatedAtAction(nameof(Create), result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UpdateProductResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand request)
    {
        var command = new UpdateProductWithIdCommand
        {
            Id = id,
            Name = request.Name,
            Price = request.Price
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand { Id = id });
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(PagedResult<GetPagedProductResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetPagedProductsCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}