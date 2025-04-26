using MediatR;
using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Models;

namespace RO.DevTest.Application.Features.Sale.Queries.GetPagedSales;

public class GetPagedSalesQueryHandler(ISaleRepository saleRepo)
    : IRequestHandler<GetPagedSalesQuery, PagedResult<SaleResult>>
{
    private readonly ISaleRepository _saleRepo = saleRepo;

    public async Task<PagedResult<SaleResult>> Handle(GetPagedSalesQuery request, CancellationToken cancellationToken)
    {
        var query = _saleRepo.Query()
            .Include(s => s.Customer)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product);

        var list = query.ToList();

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            list = request.SortBy.ToLower() switch
            {
                "customer" => request.Descending
                    ? [.. list.OrderByDescending(s => s.Customer.Name)]
                    : [.. list.OrderBy(s => s.Customer.Name)],

                "saledate" => request.Descending
                    ? [.. list.OrderByDescending(s => s.SaleDate)]
                    : [.. list.OrderBy(s => s.SaleDate)],

                "total" => request.Descending
                    ? [.. list.OrderByDescending(s => s.TotalAmount)]
                    : [.. list.OrderBy(s => s.TotalAmount)],

                _ => list
            };
        }

        var totalItems = list.Count;

        var items = list
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(s => new SaleResult
            {
                Id = s.Id.ToString(),
                CustomerName = s.Customer.Name,
                SaleDate = s.SaleDate,
                Total = s.TotalAmount,
                Items = [.. s.Items.Select(i => new SaleItemResult
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })]
            })
            .ToList();

        return new PagedResult<SaleResult>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize),
            Items = items
        };
    }
}