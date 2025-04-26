using MediatR;
using RO.DevTest.Application.Models;

namespace RO.DevTest.Application.Features.Sale.Queries.GetPagedSales;

public class GetPagedSalesCommand : IRequest<PagedResult<SaleResult>>
{
    public string? SortBy { get; set; }
    public bool Descending { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}