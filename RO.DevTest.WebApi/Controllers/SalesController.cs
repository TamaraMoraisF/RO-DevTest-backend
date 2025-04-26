using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;
using RO.DevTest.Application.Features.Sale.Queries.GetPagedSales;
using RO.DevTest.Application.Models;

namespace RO.DevTest.WebApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/sales")]
[ApiController]
public class SalesController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CreateSaleResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateSaleCommand request)
    {
        var result = await _mediator.Send(request);
        return CreatedAtAction(nameof(Create), result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SaleResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] GetPagedSalesCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

}
