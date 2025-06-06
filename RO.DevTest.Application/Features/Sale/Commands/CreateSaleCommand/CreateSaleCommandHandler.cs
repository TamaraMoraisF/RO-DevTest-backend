﻿using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Sale.Commands.CreateSaleCommand;

public class CreateSaleCommandHandler(
    ISaleRepository saleRepository,
    ICustomerRepository customerRepository,
    IProductRepository productRepository
) : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository = saleRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            throw new BadRequestException(validation);

        var customer = _customerRepository.Get(c => c.Id == request.CustomerId)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = _productRepository.Query().Where(p => productIds.Contains(p.Id)).ToList();

        if (products.Count != productIds.Count)
            throw new NotFoundException(nameof(Product), "Some products were not found");

        var saleItems = request.Items.Select(i =>
        {
            var product = products.First(p => p.Id == i.ProductId);
            return new SaleItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = product.Price
            };
        }).ToList();

        var sale = new Domain.Entities.Sale
        {
            CustomerId = request.CustomerId,
            Items = saleItems
        };

        await _saleRepository.CreateAsync(sale, cancellationToken);

        return new CreateSaleResult(
            sale.Id,
            sale.CustomerId,
            sale.SaleDate,
            sale.TotalAmount
        );
    }
}