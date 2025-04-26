namespace RO.DevTest.Application.Features.Sale.Queries.GetPagedSales
{
    public class SaleResult
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public decimal Total { get; set; }
        public List<SaleItemResult> Items { get; set; } = new();
    }
}