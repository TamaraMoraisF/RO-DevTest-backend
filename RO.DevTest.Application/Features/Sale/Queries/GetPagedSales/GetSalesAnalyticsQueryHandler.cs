using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RO.DevTest.Application.Contracts.Persistance.Repositories;

namespace RO.DevTest.Application.Features.Sale.Queries.GetPagedSales;

public class GetSalesAnalyticsQueryHandler(ISaleRepository saleRepo)
    : IRequestHandler<GetSalesAnalyticsQuery, SalesAnalyticsResult>
{
    private readonly ISaleRepository _saleRepo = saleRepo;

    public async Task<SalesAnalyticsResult> Handle(GetSalesAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var startDateUtc = DateTime.SpecifyKind(request.Start, DateTimeKind.Utc);
        var endDateUtc = DateTime.SpecifyKind(request.End, DateTimeKind.Utc);

        var query = _saleRepo.Query()
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .Where(s => s.SaleDate >= startDateUtc && s.SaleDate <= endDateUtc);

        List<Domain.Entities.Sale> sales;

        if (query.Provider is IAsyncQueryProvider) 
        {
            sales = await query.ToListAsync(cancellationToken); 
        }
        else
        {
            sales = [.. query];
        }

        var totalSales = sales.Count;
        var totalRevenue = sales.Sum(s => s.TotalAmount);

        var productRevenue = sales
            .SelectMany(s => s.Items)
            .GroupBy(i => new { i.ProductId, i.Product.Name })
            .Select(g => new ProductRevenueResult
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                QuantitySold = g.Sum(i => i.Quantity),
                Revenue = g.Sum(i => i.Quantity * i.UnitPrice)
            })
            .ToList();

        return new SalesAnalyticsResult
        {
            TotalSales = totalSales,
            TotalRevenue = totalRevenue,
            ProductRevenueBreakdown = productRevenue
        };
    }
}