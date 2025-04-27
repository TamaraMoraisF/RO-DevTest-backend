using MediatR;
using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Models;
using FluentValidation;

namespace RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;

public class GetPagedSalesCommandHandler(ISaleRepository saleRepo)
    : IRequestHandler<GetPagedSalesCommand, PagedResult<SaleResult>>
{
    private readonly ISaleRepository _saleRepo = saleRepo;

    public async Task<PagedResult<SaleResult>> Handle(GetPagedSalesCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetPagedSalesCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        IQueryable<Domain.Entities.Sale> query = _saleRepo.Query()
            .Include(s => s.Customer)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product);

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "customer" => request.Descending
                    ? query.OrderByDescending(s => s.Customer.Name)
                    : query.OrderBy(s => s.Customer.Name),
                "saledate" => request.Descending
                    ? query.OrderByDescending(s => s.SaleDate)
                    : query.OrderBy(s => s.SaleDate),
                "total" => request.Descending
                    ? query.OrderByDescending(s => s.TotalAmount)
                    : query.OrderBy(s => s.TotalAmount),
                _ => query
            };
        }

        var totalItems = query.Count();

        var items = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(s => new SaleResult
            {
                Id = s.Id.ToString(),
                CustomerName = s.Customer.Name,
                SaleDate = s.SaleDate,
                Total = s.TotalAmount,
                Items = s.Items.Select(i => new SaleItemResult
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
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
