﻿namespace RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;

public class SaleItemResult
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}