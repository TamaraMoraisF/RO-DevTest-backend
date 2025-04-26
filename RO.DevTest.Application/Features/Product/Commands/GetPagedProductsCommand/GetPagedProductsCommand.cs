using MediatR;
using RO.DevTest.Application.Models;

namespace RO.DevTest.Application.Features.Product.Commands.GetPagedProductsCommand;

public class GetPagedProductsCommand : IRequest<PagedResult<GetPagedProductResult>>
{
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool Descending { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}