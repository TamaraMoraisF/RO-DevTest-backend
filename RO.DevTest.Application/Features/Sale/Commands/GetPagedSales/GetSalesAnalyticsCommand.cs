﻿using MediatR;

namespace RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;

public class GetSalesAnalyticsCommand(DateTime start, DateTime end) : IRequest<SalesAnalyticsResult>
{
    public DateTime Start { get; init; } = start;
    public DateTime End { get; init; } = end;
}