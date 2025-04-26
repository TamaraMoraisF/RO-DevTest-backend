namespace RO.DevTest.Application.Features.Sale.Queries.GetPagedSales;

public class SalesAnalyticsResult
{
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<ProductRevenueResult> ProductRevenueBreakdown { get; set; } = [];
}

public class ProductRevenueResult
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}