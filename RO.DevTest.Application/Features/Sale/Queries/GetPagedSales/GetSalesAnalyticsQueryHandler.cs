using MediatR;
using Microsoft.EntityFrameworkCore;
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

        var sales = await _saleRepo.Query()
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .Where(s => s.SaleDate >= startDateUtc && s.SaleDate <= endDateUtc)
            .ToListAsync(cancellationToken);

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
