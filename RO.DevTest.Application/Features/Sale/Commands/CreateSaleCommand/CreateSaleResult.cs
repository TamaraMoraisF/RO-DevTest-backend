namespace RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;

public record CreateSaleResult(Guid Id, Guid CustomerId, DateTime SaleDate, decimal Total);